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

    public void OnDispatchClicked()
    {
        _depot.DispatchCourier();
    }

    void Update()
    {
        _pendingPackagesLabel.text = $"Pending Packages: {_depot?.PendingPackages}";
        _bankBalanceLabel.text = $"Bank Balance: ${_depot?.BankBalance}";
    }
}
