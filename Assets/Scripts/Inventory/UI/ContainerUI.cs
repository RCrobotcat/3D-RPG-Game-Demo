using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ContainerUI : MonoBehaviour
{
    public SlotHolder[] slots;

    public void RefreshUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].itemUI.Index = i;
            slots[i].UpdateItem();
        }
    }
}
