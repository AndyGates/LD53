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

    [SerializeField]
    UITimingBar _timeoutBar;

    public UIPackageTooltip TooltipPrefab { get; set; }

    UIPackageTooltip _tooltip;
    Package _package;

    Vector3 _initialPos;

    CanvasGroup _canvasGroup;
    RectTransform _rectTransform;

    bool _isDragging = false;
    bool _isHovering = false;
    Courier _courier;
    IUIPackageOwner _owner;

    public float Width { get => _sprite.preferredWidth; }

    public Package Package { get => _package; }

    public bool CanDrag { get; set; } = true;

    public bool DontResetOnDrop { get; set; }

    public bool IsDragging { get => _isDragging; }

    public Vector2 Position 
    { 
        get => _rectTransform.anchoredPosition; 
        set { _rectTransform.anchoredPosition = value; } 
    }

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void SetOwner(IUIPackageOwner owner)
    {
        if (_owner != null)
        {
            _owner.Remove(this);
        }
        _owner = owner;
        _owner.Add(this);
    }

    public void SetPackage(Package package)
    {
        _package = package;
        _package.OnExpired += OnExpired;

        UpdateButtonSprites();
        UpdateDetailSprite();
    }

    void OnExpired(Package package)
    {
        if (package == _package)
        {
            if (_owner != null)
            {
                _owner.Remove(this);
            }
            Destroy(gameObject);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (CanDrag == false) return;

        Canvas canvas = GetComponentsInParent<Canvas>().Last();
        CanvasScaler canvasScaler = canvas.GetComponent<CanvasScaler>();
        
        if (DontResetOnDrop == false)
        {
            Position += eventData.delta / canvasScaler.scaleFactor;
        }
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
        if (CanDrag == false) return;

        _isDragging = true;

        _initialPos = Position;
        _canvasGroup.blocksRaycasts = false;
        UpdateButtonSprites();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (CanDrag == false) return;

        _isDragging = false;

        if (DontResetOnDrop == false)
        {
            Position = _initialPos;
        }
        DontResetOnDrop = false;
        _canvasGroup.blocksRaycasts = true;
        UpdateButtonSprites();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isHovering = true;

        _tooltip = GameObject.Instantiate(TooltipPrefab, Input.mousePosition, Quaternion.identity, transform);
        _tooltip.SetPackage(_package);

        _package.Target.SetHighlightActive(true);
        
        UpdateButtonSprites();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isHovering = false;

        Destroy(_tooltip.gameObject);
        _tooltip = null;

        _package.Target.SetHighlightActive(false);

        UpdateButtonSprites();
    }

    void Update()
    {
        //TODO: Need a way to query how much time a package has remaining, or at least percentage of time elapsed.
        float timeRemaining = _package.DeliveryBy - Time.time;
        _timeoutBar.Time = Mathf.Clamp01(timeRemaining / _package.DeliveryTime);
    }

    public void Loaded(Courier courier)
    {
        _courier = courier;
    }

    public void Unloaded(Courier courier)
    {
        _courier = null;
    }
}
