using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPackage : MonoBehaviour
{
    [SerializeField]
    Image _sprite;

    public UIPackageTooltip TooltipPrefab { get; set; }

    UIPackageTooltip _tooltip;
    Package _package;

    public void SetPackage(Package package)
    {
        // TODO: Set sprite based on package size

        _package = package;
    }

    public void OnMouseEnter()
    {
        _tooltip = GameObject.Instantiate(TooltipPrefab, Input.mousePosition, Quaternion.identity, transform);
        _tooltip.SetPackage(_package);
    }

    public void OnMouseExit()
    {
        Destroy(_tooltip.gameObject);
        _tooltip = null;
    }
}
