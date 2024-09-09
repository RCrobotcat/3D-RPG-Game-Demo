using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBtn : MonoBehaviour
{
    public KeyCode actionKey;

    private SlotHolder currentSlot;

    void Awake()
    {
        currentSlot = GetComponent<SlotHolder>();
    }

    void Update()
    {
        if (Input.GetKeyDown(actionKey) && currentSlot.itemUI.GetItem())
            currentSlot.UseItem();
    }
}
