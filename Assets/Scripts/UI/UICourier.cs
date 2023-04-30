using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UICourier : MonoBehaviour, IDropHandler
{
    [SerializeField]
    UICourierBag _bag;

    [SerializeField]
    GameObject _delivering;

    [SerializeField]
    GameObject _dispatch;

    Courier _courier;

    void Awake()
    {
        _delivering.SetActive(false);
    }

    public void SetCourier(Courier courier)
    {
        _courier = courier;
        _courier.OnBackAtDepot += BackAtDepot;
        _bag.Courier = _courier;
    }

    public void BackAtDepot(Courier courier)
    {
        if (_courier == courier)
        {
            _delivering.SetActive(false);
            _dispatch.SetActive(true);
            _bag.SetCanDrag(true);
        }
    }

    public void Dispatch()
    {
        _delivering.SetActive(true);
        _dispatch.SetActive(false);
        _bag.SetCanDrag(false);
        
        _courier.Dispatch();
    }

    public void OnDrop(PointerEventData eventData)
    {
        UIPackage package = eventData.pointerDrag.GetComponent<UIPackage>();
        if (package != null && package.CanDrag)
        {
            if (_courier.AddPackage(package.Package))
            {
                package.DontResetOnDrop = true;
                package.Loaded(_courier);
                package.SetOwner(_bag);
            }
        }
    }
}
