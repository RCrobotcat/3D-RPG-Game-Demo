using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestNameBtn : MonoBehaviour
{
    public Text questNameTxt;
    public QuestData_SO currentQuestData;
    public Text questContentTxt;

    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(UpdateQuestContent);
    }

    void UpdateQuestContent()
    {
        questContentTxt.text = currentQuestData.description;
        QuestUI.Instance.SetUpRequirements(currentQuestData);
    }

    public void SetUpNameBtn(QuestData_SO questData)
    {
        currentQuestData = questData;

        if (questData.isCompleted)
            questNameTxt.text = questData.questName + " (Completed)";
        else questNameTxt.text = questData.questName;
    }
}
