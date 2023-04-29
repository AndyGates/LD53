using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Location : MonoBehaviour
{
    [SerializeField]
    Vector2Int _mapCoord;

    SpriteRenderer _sprite;
    Color _defaultColor;

    public System.Action<Location> OnSelected;

    public Vector2Int MapCoord { get => _mapCoord; }

    void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _defaultColor = _sprite.color;
    }

    void OnMouseEnter()
    {
        _sprite.color = Color.magenta;
    }

    void OnMouseExit()
    {
        _sprite.color = _defaultColor;
    }

    void OnMouseUp()
    {
        OnSelected.Invoke(this);
    }

    public bool ReceivePackage(Package package)
    {
        return package.Target.name == name;
    }
}
