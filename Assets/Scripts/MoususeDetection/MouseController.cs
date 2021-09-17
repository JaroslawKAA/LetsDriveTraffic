using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseController : MonoBehaviour
{
    public RectTransform cursor;

    private RectTransform _canvas;

    private void Awake()
    {
        _canvas = cursor.transform.parent.GetComponent<RectTransform>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 500, Color.magenta);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.CompareTag("MouseSnapper"))
            {
                MouseSnaper mouseSnaper = hit.transform.GetComponent<MouseSnaper>();
                Vector3 snappingPosition = mouseSnaper.GetSnappedPosition(hit.point);
                Vector2 mousePosition = Camera.main.WorldToScreenPoint(snappingPosition);
                cursor.anchoredPosition = new Vector2(mousePosition.x * _canvas.sizeDelta.x / Screen.width,
                    mousePosition.y * _canvas.sizeDelta.y / Screen.height);
            }
            else
            {
                cursor.anchoredPosition = new Vector2(Input.mousePosition.x * _canvas.sizeDelta.x / Screen.width,
                    Input.mousePosition.y * _canvas.sizeDelta.y / Screen.height);
            }
        }
    }
}