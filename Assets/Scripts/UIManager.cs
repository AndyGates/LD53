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
    TextMeshProUGUI _space;

    [SerializeField]
    TextMeshProUGUI _loadedPackages;

    [SerializeField]
    GameManager _gameManager;

    [SerializeField]
    UIPackages _packages;

    void Awake()
    {
        _depot.OnPackagedAdded += OnPackageAdded;
        _depot.OnPackagedLoaded += OnPackagedLoaded;
    }

    void OnPackageAdded(Package package)
    {
        _packages.AddPackage(package);
    }

    void OnPackagedLoaded(Package package)
    {

    }

    public void OnDispatchClicked()
    {
        _depot.DispatchCourier();
    }

    void Update()
    {
        _bankBalanceLabel.text = $"Bank Balance: ${_gameManager.State.BankBalance}";

        if (_depot.SelectedCourier != null)
        {
            _space.text = $"Available Space: {_depot.SelectedCourier.CalculateAvailableSpace()}";
            _loadedPackages.text = $"Loaded Packages: {_depot.SelectedCourier.LoadedPackages}";
        }
        else
        {
            _space.text = "";
            _loadedPackages.text = "";
        }
    }
}
