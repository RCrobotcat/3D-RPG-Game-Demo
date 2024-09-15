using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragPanel : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    RectTransform rectTransform;

    Canvas canvas;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = InventoryManager.Instance.GetComponent<Canvas>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        // eventData.delta is the change in position of the mouse cursor
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        rectTransform.SetSiblingIndex(2);
    }
}
