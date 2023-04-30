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

    [SerializeField]
    PauseMenu _pauseMenu;

    [SerializeField]
    UITimingBar _nextPackageProgress;

    [SerializeField]
    PackageStream _stream;

    [SerializeField]
    GameObject _hireButton;

    void Awake()
    {
        _depot.OnPackagedAdded += OnPackageAdded;
        _depot.OnPackagedLoaded += OnPackagedLoaded;
        _depot.OnCourierCreated += OnCourierCreated;

        _hireButton.SetActive(false);
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _pauseMenu.gameObject.SetActive(true);
        }

        if(_bankBalanceLabel != null)
        {
            _bankBalanceLabel.text = $"$: {_gameManager.State.BankBalance}";
        }

        _nextPackageProgress.Time = _stream.NextPackageProgress;

        _hireButton.SetActive(_depot.CanHire);
    }
}
