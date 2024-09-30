using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestRequirement : MonoBehaviour
{
    Text requirementName;
    Text progressNum;

    void Awake()
    {
        requirementName = GetComponent<Text>();
        progressNum = transform.GetChild(0).GetComponent<Text>();
    }
}
