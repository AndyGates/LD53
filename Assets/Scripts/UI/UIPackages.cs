using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIPackages : MonoBehaviour, IUIPackageOwner, IDropHandler
{
    [SerializeField]
    UIPackage _packagePrefab;

    [SerializeField]
    UIPackageTooltip _tooltipPrefab;
    
    [SerializeField]
    float _ySpacing = 60;

    List<UIPackage> _packages = new List<UIPackage>();

    public void CreatePackage(Package package)
    {
        UIPackage uiPackage = GameObject.Instantiate(_packagePrefab, transform);
        uiPackage.TooltipPrefab = _tooltipPrefab;

        uiPackage.SetPackage(package);
        uiPackage.SetOwner(this);
    }

    public void Add(UIPackage package)
    {
        if (_packages.Contains(package) == false)
        {
            package.transform.SetParent(transform, false);
            _packages.Add(package);
            UpdateSpacing();
            Debug.Log($"Adding package to packages {_packages.Count}");
        }
        else
        {
            Debug.LogWarning("Trying to add package we already have");
        }
    }

    public void Remove(UIPackage package)
    {
        _packages.Remove(package);
        UpdateSpacing();
        Debug.Log($"Removing package from packages {_packages.Count}");
    }

    void UpdateSpacing()
    {
        for(int i = 0; i < _packages.Count; i++)
        {
            _packages[i].Position = new Vector2(0.0f, -_ySpacing * i);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        UIPackage package = eventData.pointerDrag.GetComponent<UIPackage>();
        if (package != null && _packages.Contains(package) == false)
        {
            package.DontResetOnDrop = true;
            package.SetOwner(this);
        }
    }
}
