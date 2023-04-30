using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Location : MonoBehaviour
{
    [SerializeField]
    Vector2Int _mapCoord;

    [SerializeField]
    SpriteRenderer _outlineSprite;

    [SerializeField]
    SpriteRenderer _baseSprite;

    Color _defaultColor;

    public System.Action<Location> OnSelected;

    public Vector2Int MapCoord { get => _mapCoord; }

    void Awake()
    {
        _defaultColor = _baseSprite.color;
    }

    void OnMouseEnter()
    {
        _baseSprite.color = Color.magenta;
    }

    void OnMouseExit()
    {
        _baseSprite.color = _defaultColor;
    }

    void OnMouseUp()
    {
        OnSelected.Invoke(this);
    }

    public bool ReceivePackage(Package package)
    {
        return package.Target.name == name;
    }

    public void SetHighlightActive(bool active)
    {
        _outlineSprite.enabled = active;
    }
}
