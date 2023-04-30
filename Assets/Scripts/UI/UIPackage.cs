using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class UIPackage : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
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

    Vector3 _initialPos;

    CanvasGroup _canvasGroup;
    RectTransform _rectTransform;

    bool _isDragging = false;
    bool _isHovering = false;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void SetPackage(Package package)
    {
        _package = package;
        Debug.Log($"Adding ui package with size {package.Size}");

        UpdateButtonSprites();
        UpdateDetailSprite();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Canvas canvas = GetComponentsInParent<Canvas>().Last();
        CanvasScaler canvasScaler = canvas.GetComponent<CanvasScaler>();
        
        _rectTransform.anchoredPosition += eventData.delta / canvasScaler.scaleFactor;
    }

    void UpdateButtonSprites() 
    {
        bool isHighlighted = _isDragging  || _isHovering;

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

    public void OnBeginDrag(PointerEventData eventData)
    {
        _isDragging = true;

        _initialPos = _rectTransform.anchoredPosition;
        _canvasGroup.blocksRaycasts = false;
        UpdateButtonSprites();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _isDragging = false;
        
        _rectTransform.anchoredPosition = _initialPos;
        _canvasGroup.blocksRaycasts = true;
        UpdateButtonSprites();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isHovering = true;

        _tooltip = GameObject.Instantiate(TooltipPrefab, Input.mousePosition, Quaternion.identity, transform);
        _tooltip.SetPackage(_package);
        
        UpdateButtonSprites();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isHovering = false;

        Destroy(_tooltip.gameObject);
        _tooltip = null;

        UpdateButtonSprites();
    }
}
