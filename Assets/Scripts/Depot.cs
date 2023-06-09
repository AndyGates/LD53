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

    [SerializeField]
    int _maxPendingPackages = 7;

    [SerializeField]
    AudioClip _startSound;

    [SerializeField]
    int _firstHireThreshold = 50;

    [SerializeField]
    int _secondHireThreshold = 100;

    [SerializeField]
    int _compensationScale = 4;

    [SerializeField]
    int _fineAmount = 10;

    [SerializeField]
    int _winTarget = 200;

    List<Courier> _couriers = new List<Courier>();

    List<Package> _packages = new List<Package>();

    // Need to make sure if balance drops below target and pop up shown we dont remove button
    // Looks like a bug otherwise
    bool _canHireFirst = false;

    public bool CanHire 
    { 
        get 
        {
            if(_gameManager.State == null) return false;

            switch(_couriers.Count)
            {
                case 0:
                    return true;
                case 1:
                    return _gameManager.State.BankBalance >= _firstHireThreshold || _canHireFirst;
                case 2:
                    return _gameManager.State.BankBalance >= _secondHireThreshold;
                default:
                    return false;
            }
        }
    }

    bool _lastCanHire = false;
    bool _canHireShow = false;

    void Awake()
    {
        HireCourier(true);

        _lastCanHire = CanHire;

        AudioSource.PlayClipAtPoint(_startSound, Vector3.zero);
    }

    void Start()
    {
        Dialog.Show($"To win you need to reach a target bank balance of ${_winTarget}");
    }

    public void HireCourier()
    {
        HireCourier(false);
    }

    void HireCourier(bool force)
    {
        if (CanHire || force)
        {
            Courier courier = GameObject.Instantiate<Courier>(_courierPrefab);
            courier.transform.position = _map.ToWorld(_map.DepotLocation) + new Vector3(_map.Scale * 0.5f, -_map.Scale * 0.5f, 0.0f);
            courier.Map = _map;
            _couriers.Add(courier);
            OnCourierCreated.Invoke(courier);
        }
    }

    void Update()
    {
        Package incomingPackage = _packageStream.Next();
        if(incomingPackage != null)
        {
            if (CountDepotPackages() < _maxPendingPackages)
            {
                Debug.Log($"Got new package. Size: {incomingPackage.Size}, Postage; {incomingPackage.Delivery.Price}, Value: {incomingPackage.Value}, Target: {incomingPackage.Target.name}");
                _gameManager.State.BankBalance += incomingPackage.Delivery.Price;
                _packages.Add(incomingPackage);
                incomingPackage.OnDelivered += OnDelivered;

                OnPackagedAdded.Invoke(incomingPackage);

                Console.Show($"A new package arrived at the depot.");
            }
            else
            {
                Debug.Log("Cant store any more packages at the depot");
                Console.Show("Depot has run out of storage. You are being fined for a bad public service.");

                _gameManager.State.BankBalance -= _fineAmount;
            }
        }

        List<Package> toRemove = new List<Package>();
        foreach(Package package in _packages)
        {
            if (package.HasExpired())
            {
                package.OnExpired?.Invoke(package);
                toRemove.Add(package);

                Debug.Log("Package expired");

                int compensation = package.Delivery.Price * _compensationScale;

                Console.Show($"You have missed the deadline for a delivery. You need to pay the customer compensation of ${compensation}.");

                _gameManager.State.BankBalance -= compensation; // Compensation is twice the postage
            }
        }
        toRemove.ForEach(p => _packages.Remove(p));


        if (_lastCanHire != CanHire && CanHire && _canHireShow == false)
        {
            _canHireShow = true;
            _canHireFirst = true;
            Dialog.Show("Well done, you can now hire an additional courier. Click the the hire button next to your balance to hire a extra courier.");
        }
        _lastCanHire = CanHire;

        if (_gameManager.State.BankBalance < 0)
        {
            _gameManager.EndGame("You ran out of money and can no long operate your delivery business.");
        }

        if (_gameManager.State.BankBalance >= _winTarget)
        {
            _gameManager.EndGame($"You reached you target of ${_winTarget}, well done.");
        }
    }

    void OnDelivered(Package package)
    {
        _packages.Remove(package);

        //Remove (clone) because we're using GO names rather than data... 
        string targetName = package.Target.name;
        string cloneStr = "(Clone)";
        if (targetName.EndsWith(cloneStr))
        {
            targetName = targetName.Substring(0, targetName.Length - cloneStr.Length);
        }

        Console.Show($"Package successfully delivered to {targetName}");
    }

    int CountDepotPackages()
    {
        return _packages.Count - _couriers.Sum(c => c.LoadedPackages);
    }
}
