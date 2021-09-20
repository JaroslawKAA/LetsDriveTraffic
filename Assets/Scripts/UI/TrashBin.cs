using System;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.EventSystems;

public class TrashBin : MonoBehaviour
{
    private SVGImage _svg;

    public bool IsCursorOver { get; private set; }
    
    private void Activate()
    {
        _svg.color = new Color(_svg.color.r, _svg.color.g, _svg.color.b, 1f);
    }

    private void Deactivate()
    {
        _svg.color = new Color(_svg.color.r, _svg.color.g, _svg.color.b, .35f);
    }

    private void Start()
    {
        GameEvents.S.OnDraggingSignStart += OnDraggingSignStart;
        GameEvents.S.OnDraggingSignEnd += OnDraggingSignEnd;
        gameObject.SetActive(false);

        _svg = GetComponent<SVGImage>();
    }

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            Activate();
            IsCursorOver = true;
        }
        else
        {
            Deactivate();
            IsCursorOver = false;
        }
    }

    private void OnDraggingSignStart()
    {
        gameObject.SetActive(true);
    }

    private void OnDraggingSignEnd()
    {
        gameObject.SetActive(false);
    }
}