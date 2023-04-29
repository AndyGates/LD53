using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Location : MonoBehaviour
{
    SpriteRenderer _sprite;
    Color _defaultColor;

    public System.Action<string> OnSelected;

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
        OnSelected.Invoke(name);
    }
}
