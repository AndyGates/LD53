using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    Depot _depot;

    public void OnDispatchClicked()
    {
        _depot.DispatchCourier();
    }
}
