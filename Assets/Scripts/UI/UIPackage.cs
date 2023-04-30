using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIPackage : MonoBehaviour, IDragHandler
{
    [System.Serializable]
    struct SpriteSet 
    {
        public Sprite[] _sprites;

        public Sprite GetRandom() 
        {
            return _sprites[Random.Range(0, _sprites.Length)];
        }
    }

    [SerializeField]
    Sprite[] _buttonSprites;

    [SerializeField]
    Sprite[] _highlightSprites;

    [SerializeField]
    SpriteSet[] _packageSprites;

    [SerializeField]
    Image _sprite;

    [SerializeField]
    Image _detailSprite;

    public UIPackageTooltip TooltipPrefab { get; set; }

    UIPackageTooltip _tooltip;
    Package _package;

    public void SetPackage(Package package)
    {
        _package = package;
        Debug.Log($"Adding ui package with size {package.Size}");

        UpdateButtonSprites(false);
        UpdateDetailSprite();
    }

    public void OnMouseEnter()
    {
        UpdateButtonSprites(true);
        _tooltip = GameObject.Instantiate(TooltipPrefab, Input.mousePosition, Quaternion.identity, transform);
        _tooltip.SetPackage(_package);
    }

    public void OnMouseExit()
    {
        UpdateButtonSprites(false);
        Destroy(_tooltip.gameObject);
        _tooltip = null;
    }

    public void OnBeginDrag() {
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    void UpdateButtonSprites(bool isHighlighted) 
    {
        int size = _package.Size-1;
        _sprite.sprite = isHighlighted ? _highlightSprites[size] : _buttonSprites[size];
        _sprite.SetNativeSize();
    }

        void UpdateDetailSprite() 
    {
        int size = _package.Size-1;
        _detailSprite.sprite = _packageSprites[size].GetRandom();
        _detailSprite.SetNativeSize();
    }
}
