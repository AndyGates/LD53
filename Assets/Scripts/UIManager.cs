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
    TextMeshProUGUI _pendingPackagesLabel;

    [SerializeField]
    TextMeshProUGUI _bankBalanceLabel;

    [SerializeField]
    TextMeshProUGUI _targetLocationLabel;

    [SerializeField]
    TextMeshProUGUI _postageLabel;

    [SerializeField]
    TextMeshProUGUI _valueLabel;

    [SerializeField]
    TextMeshProUGUI _sizeLabel;

    [SerializeField]
    TextMeshProUGUI _space;

    [SerializeField]
    TextMeshProUGUI _loadedPackages;

    public void OnDispatchClicked()
    {
        _depot.DispatchCourier();
    }

    void Update()
    {
        _pendingPackagesLabel.text = $"Pending Packages: {_depot?.PendingPackages}";
        _bankBalanceLabel.text = $"Bank Balance: ${_depot?.BankBalance}";

        if (_depot.PendingPackage != null)
        {
            _targetLocationLabel.text = $"Target Location: {_depot.PendingPackage.Target.name}";
            _postageLabel.text = $"Postage: ${_depot.PendingPackage.Postage}";
            _valueLabel.text = $"Value: ${_depot.PendingPackage.Value}";
            _sizeLabel.text = $"Size: {_depot.PendingPackage.Size}";
        }
        else
        {
            _targetLocationLabel.text = "";
            _postageLabel.text = "";
            _valueLabel.text = "";
            _sizeLabel.text = "";
        }

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
