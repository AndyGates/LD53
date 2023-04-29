using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Depot : MonoBehaviour
{
    [SerializeField]
    Map _map;

    [SerializeField]
    List<Location> _locations = new List<Location>();

    [SerializeField]
    Courier _courierPrefab;

    List<Courier> _couriers = new List<Courier>();

    Courier _selectedCourier = null;

    Queue<Package> _packages = new Queue<Package>();
    Package _selectedPackage = null;

    void Awake()
    {
        foreach(Location location in _locations)
        {
            location.OnSelected += LocationSelected;
        }

        _selectedCourier = GameObject.Instantiate<Courier>(_courierPrefab);
        _couriers.Add(_selectedCourier);
    }

    void LocationSelected(string name)
    {
        if (_selectedCourier.IsAtDepot)
        {
            if (_selectedPackage == null)
            {
                Debug.Log("No packages to deliver");
            }
            else
            {
                _selectedCourier.AddPackage(_selectedPackage);
                NextPackage();
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
        }
        else
        {
            Debug.LogWarning("No courier to dispatch");
        }
    }

    public void AddPackage(Package package)
    {
        _packages.Enqueue(package);
    }
}
