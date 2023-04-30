using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    Depot _depot;

    [SerializeField]
    TextMeshProUGUI _bankBalanceLabel;

    [SerializeField]
    GameManager _gameManager;

    [SerializeField]
    UIPackages _packages;

    [SerializeField]
    UICouriers _couriers;

    void Awake()
    {
        _depot.OnPackagedAdded += OnPackageAdded;
        _depot.OnPackagedLoaded += OnPackagedLoaded;
        _depot.OnCourierCreated += OnCourierCreated;
    }

    void OnPackageAdded(Package package)
    {
        _packages.CreatePackage(package);
    }

    void OnPackagedLoaded(Package package)
    {

    }

    void OnCourierCreated(Courier courier)
    {
        _couriers.AddCourier(courier);
    }

    void Update()
    {
        if(_bankBalanceLabel != null)
        {
            _bankBalanceLabel.text = $"$: {_gameManager.State.BankBalance}";
        }
    }
}
