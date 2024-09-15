using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class ItemTooptip : MonoBehaviour
{
    public Text itemName;
    public Text itemDescription;

    RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetUpTooltip(ItemData_SO item)
    {
        itemName.text = item.itemName;
        itemDescription.text = item.description;
    }

    void OnEnable()
    {
        UpdatePosition();
    }

    void Update()
    {
        UpdatePosition();
    }

    public void UpdatePosition()
    {
        Vector3 mousePos = Input.mousePosition;

        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners); // get the corners of the tooltip

        float width = corners[3].x - corners[0].x;
        float height = corners[1].y - corners[0].y;

        if (mousePos.y < height)
            rectTransform.position = mousePos + Vector3.up * height * 0.6f;
        else if (Screen.width - mousePos.x < width)
            rectTransform.position = mousePos + Vector3.right * width * 0.6f;
        else
            rectTransform.position = mousePos + Vector3.left * width * 0.6f;
    }
}
