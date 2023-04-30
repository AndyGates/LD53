using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICouriers : MonoBehaviour
{
    [SerializeField]
    UICourier _courierPrefab;

    [SerializeField]
    float _ySpacing = 60;

    List<UICourier> _couriers = new List<UICourier>();

    public void AddCourier(Courier courier)
    {
        UICourier uiCourier = GameObject.Instantiate(_courierPrefab, transform);
        RectTransform rect = uiCourier.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(0.0f, -_ySpacing * _couriers.Count);
        _couriers.Add(uiCourier);

        uiCourier.SetCourier(courier);
    }
}
