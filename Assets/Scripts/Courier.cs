using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Courier : MonoBehaviour
{
    public System.Action<Courier> OnBackAtDepot;

    public int LoadedPackages { get => _packages.Count; }

    public Map Map { get; set; }

    Vector3 PosOffset { get => new Vector3(Map.Scale * 0.5f, -Map.Scale * 0.5f, 0.0f); }

    [SerializeField]
    int _space = 4;

    List<Package> _packages = new List<Package>();

    Vector2Int _mapCoord;
    Package _selectedPackage;
    bool _dispatched = false;
    Route _currentRoute;

    public bool IsAtDepot
    {
        get { return Map == null || MathHelper.Approximately(Map.ToWorld(Map.DepotLocation) + PosOffset, transform.position); }
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

        _mapCoord = _currentRoute.Path.ElementAt(Mathf.Clamp(_pathIndex, 0, _currentRoute.Path.Count() - 1));
        Vector3 target = Map.ToWorld(_mapCoord) + PosOffset;
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
                _selectedPackage.Target.ReceivePackage(_selectedPackage);
                Debug.Log("Packaged successfully delivered");
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
            _selectedPackage = _packages.First();
            _currentRoute = Map.GenerateRoute(_mapCoord, _selectedPackage.Target.MapCoord);
            _pathIndex = 0;

            Debug.Log($"Courier heading to {_selectedPackage.Target.name}");
        }
        else if(IsAtDepot == false)
        {
            _selectedPackage = null;
            _currentRoute = Map.GenerateRoute(_mapCoord, Map.DepotLocation);
            _pathIndex = 0;
            _deliveringWait = 1.0f;
            Debug.Log("Heading back to depot");
        } 
        else
        {
            _dispatched = false;
            Debug.Log("Back at depot");
            OnBackAtDepot?.Invoke(this);
        }
    }

    public int CalculateAvailableSpace()
    {
        return _space - _packages.Sum(p => p.Size);
    }

    public bool AddPackage(Package package)
    {
        if (CalculateAvailableSpace() >= package.Size && IsAtDepot)
        {
            _packages.Add(package);
            return true;
        }
        return false;
    }

    public void RemovePackage(Package package)
    {
        _packages.Remove(package);
    }

    public void Dispatch()
    {
        _dispatched = true;
        _mapCoord = Map.DepotLocation;

        Debug.Log($"Courier dispatching with {_packages.Count} packages");

        NextPackage();
    }

    void OnDrawGizmos()
    {
        if(_currentRoute == null || Map == null)
        {
            return;
        }

        foreach(Vector2Int wayPoint in _currentRoute.Path)
        {
            Gizmos.color = Color.blue;
            Vector3 pos = Map.ToWorld(wayPoint);
            Gizmos.DrawSphere(pos + new Vector3(Map.Scale * 0.5f, -Map.Scale * 0.5f, 0.0f), 0.5f);
        }
    }
}
