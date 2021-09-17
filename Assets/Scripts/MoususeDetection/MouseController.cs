using System;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    /// <summary>
    /// Singleton
    /// </summary>
    public static MouseController S;

    public LayerMask mouseHitMask;

    [SerializeField] private Cursor cursor;

    public Cursor Cursor
    {
        get => cursor;
        private set => cursor = value;
    }

    private GameObject selectedItem;

    /// <summary>
    /// Canvas with cursor.
    /// </summary>
    private RectTransform _canvas;

    private void Awake()
    {
        if (S != null)
            throw new Exception("Try to create second singleton object.");
        S = this;

        _canvas = cursor.transform.parent.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 500, Color.magenta);
        if (Physics.Raycast(ray, out hit, 5000, mouseHitMask))
        {
            if (hit.transform.CompareTag("MouseSnapper"))
            {
                MouseSnapper mouseSnapper = hit.transform.GetComponent<MouseSnapper>();
                Vector3 snappingPosition = mouseSnapper.GetSnappedPosition(hit.point);
                Vector2 mousePosition = Camera.main.WorldToScreenPoint(snappingPosition);
                cursor.GetComponent<RectTransform>().anchoredPosition = new Vector2(
                    mousePosition.x * _canvas.sizeDelta.x / Screen.width,
                    mousePosition.y * _canvas.sizeDelta.y / Screen.height);

                if (selectedItem != null)
                {
                    Cursor.ActivateCursor();
                }

                if (Input.GetMouseButtonDown(0) && selectedItem != null)
                {
                    Instantiate(selectedItem, snappingPosition, Quaternion.identity);
                }
            }
            else
            {
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
        }

        if (Input.GetMouseButtonDown(1))
        {
            ResetSelectedItem();
            Cursor.ResetIcon();
        }
    }

    public void SelectItem(GameObject prefab)
    {
        selectedItem = prefab;
        Sprite signSprite = prefab.GetComponent<Sign>().SignSprite;
        Cursor.SetItemIcon(signSprite);
    }

    private void ResetSelectedItem()
    {
        selectedItem = null;
    }
}