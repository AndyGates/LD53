using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Depot : MonoBehaviour
{
    public int PendingPackages { get => _packages.Count + (_selectedPackage == null ? 0 : 1); }
    public int BankBalance { get; private set; } = 0;

    public Package PendingPackage { get => _selectedPackage; }
    public Courier SelectedCourier { get => _selectedCourier; }

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
    bool _incorrectTargetLocationWarned = false;

    void Awake()
    {
        foreach(Location location in _map.Locations)
        {
            location.OnSelected += LocationSelected;
        }

        _selectedCourier = GameObject.Instantiate<Courier>(_courierPrefab);
        _selectedCourier.transform.position = _map.ToWorld(_map.DepotLocation) + new Vector3(_map.Scale * 0.5f, -_map.Scale * 0.5f, 0.0f);
        _couriers.Add(_selectedCourier);
    }

    void Update()
    {
        Package package = _packageStream.Next();
        if(package != null)
        {
            Debug.Log($"Got new package. Size: {package.Size}, Postage; {package.Delivery.Price}, Value: {package.Value}, Target: {package.Target.name}");
            BankBalance += package.Delivery.Price;
            _packages.Enqueue(package);

            if (_selectedPackage == null)
            {
                NextPackage();
            }
        }

        foreach(Courier courier in _couriers)
        {
            if (courier.IsAtDepot && courier.IsDispatched == false)
            {
                if (_selectedCourier == null)
                {
                    _selectedCourier = courier;
                    Debug.Log("Got new courier");
                }
                
                if(courier.Undelivered.Count > 0)
                {
                    int undeliveredCost = courier.Undelivered.Sum(p => p.Value);
                    BankBalance -= undeliveredCost;
                    if(BankBalance < 0)
                    {
                        Debug.Log("Game over. No monies left.");
                        SceneManager.LoadScene("Done");
                    }
                }
                courier.Clear();
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
                if (_incorrectTargetLocationWarned == false && _selectedPackage.Target.name != location.name)
                {
                    Dialog.Show("You have selected to send this package to the incorrect location. This would result in you having to reimbursed the customer for the value of the item. You will not be warned again.");
                    _incorrectTargetLocationWarned = true;
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
