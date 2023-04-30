using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Depot : MonoBehaviour
{
    public int PendingPackages { get => _packages.Count; }

    public System.Action<Package> OnPackagedAdded;
    public System.Action<Package> OnPackagedLoaded;
    public System.Action<Courier> OnCourierCreated;

    [SerializeField]
    Map _map;

    [SerializeField]
    Courier _courierPrefab;

    [SerializeField]
    PackageStream _packageStream;

    [SerializeField]
    GameManager _gameManager;

    List<Courier> _couriers = new List<Courier>();

    Queue<Package> _packages = new Queue<Package>();

    void Awake()
    {
        CreateCourier();
    }

    Courier CreateCourier()
    {
        Courier courier = GameObject.Instantiate<Courier>(_courierPrefab);
        courier.transform.position = _map.ToWorld(_map.DepotLocation) + new Vector3(_map.Scale * 0.5f, -_map.Scale * 0.5f, 0.0f);
        courier.Map = _map;
        _couriers.Add(courier);
        OnCourierCreated.Invoke(courier);
        return courier;
    }

    void Update()
    {
        Package package = _packageStream.Next();
        if(package != null)
        {
            Debug.Log($"Got new package. Size: {package.Size}, Postage; {package.Delivery.Price}, Value: {package.Value}, Target: {package.Target.name}");
            _gameManager.State.BankBalance += package.Delivery.Price;
            _packages.Enqueue(package);

            OnPackagedAdded.Invoke(package);
        }

        foreach(Courier courier in _couriers)
        {
            if (courier.IsAtDepot && courier.IsDispatched == false)
            {
                // TODO: Handle missed delivery windows??
            }
        }
    }
}
