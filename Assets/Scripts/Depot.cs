using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Depot : MonoBehaviour
{
    [SerializeField]
    Map _map;

    [SerializeField]
    Courier _courierPrefab;

    [SerializeField]
    PackageStream _packageStream;

    List<Courier> _couriers = new List<Courier>();

    Courier _selectedCourier = null;

    Queue<Package> _packages = new Queue<Package>();
    Package _selectedPackage = null;

    void Awake()
    {
        foreach(Location location in _map.Locations)
        {
            location.OnSelected += LocationSelected;
        }

        _selectedCourier = GameObject.Instantiate<Courier>(_courierPrefab);
        _selectedCourier.transform.position = _map.ToWorld(_map.DepotLocation);
        _couriers.Add(_selectedCourier);
    }

    void Update()
    {
        Package package = _packageStream.Next();
        if(package != null)
        {
            Debug.Log($"Got new package. Size: {package.Size}, Postage; {package.Postage}, Value: {package.Value}, Target: {package.Target.name}");
            _packages.Enqueue(package);

            if (_selectedPackage == null)
            {
                NextPackage();
            }
        }

        // If we dont have a courier keep checking until one has come back
        if (_selectedCourier == null)
        {
            foreach(Courier courier in _couriers)
            {
                if (courier.IsAtDepot && courier.IsDispatched == false)
                {
                    _selectedCourier = courier;
                    Debug.Log("Got new courier");
                    break;
                }
            }
        }
    }

    void LocationSelected(Location location)
    {
        if (_selectedCourier != null && _selectedCourier.IsAtDepot)
        {
            if (_selectedPackage == null)
            {
                Debug.Log("No packages to deliver");
            }
            else
            {
                if (_selectedCourier.AddPackage(_selectedPackage, location))
                {
                    NextPackage();
                }
                else
                {
                    Debug.Log("Courier does not have enough space");
                }
            }
        }
        else
        {
            Debug.Log("All couriers busy");
        }
    }

    void NextPackage()
    {
        if (_packages.Count > 0)
        {
            _selectedPackage = _packages.Dequeue();
        }
        else
        {
            _selectedPackage = null;
        }
    }

    public void DispatchCourier()
    {
        if (_selectedCourier != null)
        {
            _selectedCourier.Dispatch(_map);
            _selectedCourier = null;
        }
        else
        {
            Debug.Log("No courier to dispatch");
        }
    }
}
