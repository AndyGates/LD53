using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPackages : MonoBehaviour
{
    [SerializeField]
    UIPackage _packagePrefab;

    [SerializeField]
    UIPackageTooltip _tooltipPrefab;
    
    [SerializeField]
    float _ySpacing = 60;

    List<UIPackage> _packages = new List<UIPackage>();

    public void AddPackage(Package package)
    {
        UIPackage uiPackage = GameObject.Instantiate(_packagePrefab, transform);
        uiPackage.TooltipPrefab = _tooltipPrefab;
        RectTransform rect = uiPackage.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(0.0f, -_ySpacing * _packages.Count);
        _packages.Add(uiPackage);

        uiPackage.SetPackage(package);
    }
}
