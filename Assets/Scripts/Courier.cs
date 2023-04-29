using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Courier : MonoBehaviour
{
    struct InTransitPackage
    {
        public Package Package;
        public Location DeliveryLocation;
    }

    public List<Package> Undelivered { get => _undelivered; }
    public List<Package> Delivered { get => _delivered; }
    public int LoadedPackages { get => _packages.Count; }

    Vector3 PosOffset { get => new Vector3(_map.Scale * 0.5f, -_map.Scale * 0.5f, 0.0f); }

    [SerializeField]
    int _space = 10;

    Map _map;
    Queue<InTransitPackage> _packages = new Queue<InTransitPackage>();

    Vector2Int _mapCoord;
    InTransitPackage? _selectedPackage;
    bool _dispatched = false;
    Route _currentRoute;

    List<Package> _undelivered = new List<Package>();
    List<Package> _delivered = new List<Package>();

    public bool IsAtDepot
    {
        get { return _map == null || MathHelper.Approximately(_map.ToWorld(_map.DepotLocation) + PosOffset, transform.position); }
    }

    public bool IsDispatched { get => _dispatched; }

    int _pathIndex = 0;

    float _deliveringWait = 0.0f;

    void Update()
    {
        if (_dispatched == false || _currentRoute == null)
        {
            return;
        }

        if (_deliveringWait > 0.0f)
        {
            _deliveringWait -= Time.deltaTime;
            return;
        }

        // TODO: Handle multiple path points
        _mapCoord = _currentRoute.Path.ElementAt(Mathf.Clamp(_pathIndex, 0, _currentRoute.Path.Count() - 1));
        Vector3 target = _map.ToWorld(_mapCoord) + PosOffset;
        Vector3 direction = (target - transform.position).normalized;
        float velocity = _currentRoute.Distance / _currentRoute.Time;

        if (MathHelper.Approximately(target, transform.position) == false)
        {
            transform.Translate(direction * velocity * Time.deltaTime, Space.World);
        }
        else if(_pathIndex < _currentRoute.Path.Count())
        {
            _pathIndex++;
        }
        else
        {
            if (_selectedPackage != null)
            {
                if(_selectedPackage.Value.DeliveryLocation.ReceivePackage(_selectedPackage.Value.Package))
                {
                    _delivered.Add(_selectedPackage.Value.Package);
                    Debug.Log("Packaged successfully delivered");
                }
                else
                {
                    _undelivered.Add(_selectedPackage.Value.Package);
                    Debug.Log("Package was rejected");
                }
            }
            NextPackage();
        }
    }

    void NextPackage()
    {
        if (_packages.Count > 0)
        {
            if (_selectedPackage != null)
            {
                _deliveringWait = 1.0f;
            }
            _selectedPackage = _packages.Dequeue();
            _currentRoute = _map.GenerateRoute(_mapCoord, _selectedPackage.Value.DeliveryLocation.MapCoord);
            _pathIndex = 0;

            Debug.Log($"Courier heading to {_selectedPackage.Value.DeliveryLocation.name}");
        }
        else if(IsAtDepot == false)
        {
            _selectedPackage = null;
            _currentRoute = _map.GenerateRoute(_mapCoord, _map.DepotLocation);
            _pathIndex = 0;
            _deliveringWait = 1.0f;
            Debug.Log("Heading back to depot");
        } 
        else
        {
            _dispatched = false;
            _map = null;
            Debug.Log("Back at depot");
        }
    }

    public int CalculateAvailableSpace()
    {
        return _space - _packages.Sum(p => p.Package.Size);
    }

    public bool AddPackage(Package package, Location deliveryLocation)
    {
        if (CalculateAvailableSpace() >= package.Size)
        {
            Debug.Log("Added package to courier");
            _packages.Enqueue(new InTransitPackage(){ Package = package, DeliveryLocation = deliveryLocation });
            return true;
        }
        return false;
    }

    public void Dispatch(Map map)
    {
        _map = map;
        _dispatched = true;
        _mapCoord = _map.DepotLocation;

        Debug.Log($"Courier dispatching with {_packages.Count} packages");

        NextPackage();
    }

    public void Clear()
    {
        _delivered.Clear();
        _undelivered.Clear();
    }

    void OnDrawGizmos()
    {
        if(_currentRoute == null || _map == null)
        {
            return;
        }

        foreach(Vector2Int wayPoint in _currentRoute.Path)
        {
            Gizmos.color = Color.blue;
            Vector3 pos = _map.ToWorld(wayPoint);
            Gizmos.DrawSphere(pos + new Vector3(_map.Scale * 0.5f, -_map.Scale * 0.5f, 0.0f), 0.5f);
        }
    }
}
