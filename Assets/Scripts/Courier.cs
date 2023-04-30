using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class Courier : MonoBehaviour
{
    public System.Action<Courier> OnBackAtDepot;

    public int LoadedPackages { get => _packages.Count; }

    public Map Map { get; set; }

    Vector3 PosOffset { get => new Vector3(Map.Scale * 0.5f, -Map.Scale * 0.5f, 0.0f); }

    [SerializeField]
    public Sprite _courierLeft;
    [SerializeField]
    public Sprite _courierRight;
    [SerializeField]
    public Sprite _courierStraight;

    public float angle;
    private SpriteRenderer _renderer;
    private Sprite CurrentSprite;
    private Vector3 NewDirection;
    private Vector3 LastDirection;

    public void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    IEnumerator ChangeSpriteDirection()
    {

        CurrentSprite = _courierStraight;
        if (NewDirection.x > 0)
        {
            CurrentSprite = _courierLeft;
        }
        else if (NewDirection.x < 0)
        {
            CurrentSprite = _courierRight;
        }
        _renderer.sprite = CurrentSprite;
        yield return null;
    }

    [SerializeField]
    int _space = 4;

    [SerializeField]
    AudioClip _packageAddedSound;

    [SerializeField]
    AudioClip _cantAddPackageSound;

    [SerializeField]
    AudioClip _dispatchSound;

    [SerializeField]
    AudioClip _deliveredSound;

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
        float velocity = _currentRoute.Distance / _currentRoute.Time;
        
        NewDirection = (target - transform.position).normalized;
        StartCoroutine(ChangeSpriteDirection());
        


        

        if (MathHelper.Approximately(target, transform.position) == false)
        {
            transform.Translate(NewDirection * velocity * Time.deltaTime, Space.World);
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
                AudioSource.PlayClipAtPoint(_deliveredSound, Vector3.zero);
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

            AudioSource.PlayClipAtPoint(_packageAddedSound, Vector3.zero);
            return true;
        }
        AudioSource.PlayClipAtPoint(_cantAddPackageSound, Vector3.zero);
        return false;
    }

    public void RemovePackage(Package package)
    {
        _packages.Remove(package);
        AudioSource.PlayClipAtPoint(_packageAddedSound, Vector3.zero);
    }

    public void Dispatch()
    {
        _dispatched = true;
        _mapCoord = Map.DepotLocation;

        AudioSource.PlayClipAtPoint(_dispatchSound, Vector3.zero);

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
