using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Depot : MonoBehaviour
{
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

    List<Package> _packages = new List<Package>();

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
        Package incomingPackage = _packageStream.Next();
        if(incomingPackage != null)
        {
            Debug.Log($"Got new package. Size: {incomingPackage.Size}, Postage; {incomingPackage.Delivery.Price}, Value: {incomingPackage.Value}, Target: {incomingPackage.Target.name}");
            _gameManager.State.BankBalance += incomingPackage.Delivery.Price;
            _packages.Add(incomingPackage);
            incomingPackage.OnDelivered += OnDelivered;

            OnPackagedAdded.Invoke(incomingPackage);
        }

        List<Package> toRemove = new List<Package>();
        foreach(Package package in _packages)
        {
            if (package.HasExpired())
            {
                package.OnExpired?.Invoke(package);
                toRemove.Add(package);

                Debug.Log("Package expired");

                // TODO: What should happen now
            }
        }
        toRemove.ForEach(p => _packages.Remove(p));
    }

    void OnDelivered(Package package)
    {
        _packages.Remove(package);
    }
}
