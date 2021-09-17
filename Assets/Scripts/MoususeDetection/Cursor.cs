using UnityEngine;
using UnityEngine.UI;

public class Cursor : MonoBehaviour
{
    public float Size
    {
        get => _size;
        private set => _size = value;
    }

    /// <summary>
    /// Image size in drag and drop
    /// </summary>
    public float ItemSize
    {
        get => _itemSize;
        private set => _itemSize = value;
    }

    [SerializeField] private float _size;
    [SerializeField] private float _itemSize;
    private RectTransform _rectTransform;
    private Image _image;
    private Sprite _cursorIcon;

    // Start is called before the first frame update
    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _image = GetComponent<Image>();
        _cursorIcon = _image.sprite;

        Size = _rectTransform.sizeDelta.x;
    }

    public void SetItemIcon(Sprite itemIcon)
    {
        _image.sprite = itemIcon;
        _rectTransform.sizeDelta = new Vector2(ItemSize, ItemSize);
    }

    public void ResetIcon()
    {
        _image.sprite = _cursorIcon;
        _rectTransform.sizeDelta = new Vector2(Size, Size);
    }

    public void DeactivateCursor()
    {
        _image.color = new Color(_image.color.r,
            _image.color.g,
            _image.color.b,
            .35f);
    }

    public void ActivateCursor()
    {
        _image.color = new Color(_image.color.r,
            _image.color.g,
            _image.color.b,
            1f);
    }
}