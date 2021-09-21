using System;
using Scripts;
using Signs;
using UI;
using UnityEngine;

public class MouseController : Singleton<MouseController>
{
    #region Fields

    [SerializeField] private LayerMask mouseMask;
    [SerializeField] private LayerMask placingSignMask;

    [SerializeField] private Cursor cursor;

    [Header("Defined dynamically")] [SerializeField]
    private GameObject selectedItem;

    /// <summary>
    /// Canvas with cursor.
    /// </summary>
    private RectTransform _canvas;

    private MouseSnapper _previousSnapper;

    #endregion

    public Cursor Cursor
    {
        get => cursor;
        private set => cursor = value;
    }

    protected override void OnAwake()
    {
        _canvas = cursor.transform.parent.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool isHit = Physics.Raycast(ray, out hit, 5000, mouseMask);
        bool isCursorSnapped = SnapCursor(out MouseSnapper snapper, out Vector3 snappedPosition);

        if (GameManager.S.Mode == InteractionMode.PlacingSign)
        {
            // Add sign
            if (Input.GetMouseButtonDown(0) && isCursorSnapped)
            {
                InstantiateSelectedSign(snapper, snappedPosition);
            }
            // Cancel adding sign
            else if (Input.GetMouseButtonDown(1))
            {
                DeselectedItem();
                Cursor.Reset();
                GameEvents.S.OnPlacingSignEnd_Invoke();
            }
        }
        else if (GameManager.S.Mode == InteractionMode.DraggingSign)
        {
            if (Input.GetMouseButtonUp(0))
            {
                Cursor.Show();
                if (UIManager.S.IsCursorOverBin)
                {
                    Destroy(selectedItem);
                }
                else
                {
                    selectedItem.GetComponent<SignBase>().SetContext(_previousSnapper);
                }

                DeselectedItem();
                GameEvents.S.OnDraggingSignEnd_Invoke();
            }
            else if (Input.GetMouseButton(0))
            {
                Cursor.Hide();
                if (isCursorSnapped)
                {
                    selectedItem.GetComponent<SignBase>().Drag(snappedPosition);
                }
            }
        }
        else
        {
            // Dragging item
            if (Input.GetMouseButtonDown(0)
                && isHit)
            {
                if (hit.transform.TryGetComponent(out SignBase sign))
                {
                    GameEvents.S.OnDraggingSignStart_Invoke();
                    SelectItem(hit.transform.gameObject, false);
                }
            }
        }
    }

    private bool SnapCursor(out MouseSnapper snapper, out Vector3 snappedPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 500, Color.magenta);
        if (Physics.Raycast(ray, out var hit, 5000, placingSignMask))
        {
            if (hit.transform.TryGetComponent(out snapper))
            {
                snappedPosition = snapper.GetSnappedPosition(hit.point);
                _previousSnapper = snapper;

                Vector2 mousePosition = Camera.main.WorldToScreenPoint(snappedPosition);

                cursor.GetComponent<RectTransform>().anchoredPosition = new Vector2(
                    mousePosition.x * _canvas.sizeDelta.x / Screen.width,
                    mousePosition.y * _canvas.sizeDelta.y / Screen.height);

                if (selectedItem != null)
                {
                    Cursor.ActivateCursor();
                }

                return true;
            }

            cursor.GetComponent<RectTransform>().anchoredPosition = new Vector2(
                Input.mousePosition.x * _canvas.sizeDelta.x / Screen.width,
                Input.mousePosition.y * _canvas.sizeDelta.y / Screen.height);

            if (selectedItem == null)
            {
                Cursor.ActivateCursor();
            }
            else
            {
                Cursor.DeactivateCursor();
            }
        }

        snapper = null;
        snappedPosition = Vector3.zero;

        return false;
    }

    public void SelectItem(GameObject prefab, bool setCursorSprite = true)
    {
        selectedItem = prefab;
        if (setCursorSprite)
        {
            Sprite signSprite = prefab.GetComponent<SignBase>().SignSprite;
            Cursor.SetItemIcon(signSprite);
        }
    }

    private void DeselectedItem()
    {
        selectedItem = null;
    }

    private void InstantiateSelectedSign(MouseSnapper mouseSnapper, Vector3 position)
    {
        GameObject instance = Instantiate(selectedItem, position, Quaternion.identity);
        instance.GetComponent<SignBase>().SetContext(mouseSnapper);
    }
}