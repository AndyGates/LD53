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

    public System.Action<Location> OnSelected;

    public Vector2Int MapCoord { get => _mapCoord; }

    void OnMouseUp()
    {
        OnSelected?.Invoke(this);
    }

    public void ReceivePackage(Package package)
    {
        package.OnDelivered?.Invoke(package);
    }

    public void SetHighlightActive(bool active)
    {
        _outlineSprite.enabled = active;
    }
}
