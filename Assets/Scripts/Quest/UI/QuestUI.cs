using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : Singleton<QuestUI>
{
    [Header("Elements")]
    public GameObject questPanel;
    public GameObject itemTooltip;
    bool isOpen;

    [Header("Quest Name")]
    public RectTransform questListTransform;
    public QuestNameBtn questNameBtn;

    [Header("Quest Content")]
    public Text questContentTxt;

    [Header("Requirements")]
    public RectTransform requirementsTransform;
    public QuestRequirement questRequirement;

    [Header("Rewards")]
    public RectTransform rewardsTransform;
    public ItemUI rewardUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            isOpen = !isOpen;
            questPanel.SetActive(isOpen);
            questContentTxt.text = string.Empty;

            if (!isOpen)
                itemTooltip.gameObject.SetActive(false);

            SetUpQuestList();
        }
    }

    public void SetUpQuestList()
    {
        foreach (Transform item in questListTransform)
            Destroy(item.gameObject);

        foreach (Transform item in requirementsTransform)
            Destroy(item.gameObject);

        foreach (Transform item in rewardsTransform)
            Destroy(item.gameObject);

        foreach (var task in QuestManager.Instance.questTasks)
        {
            var newTask = Instantiate(questNameBtn, questListTransform);
            newTask.SetUpNameBtn(task.questData);
            newTask.questContentTxt = questContentTxt;
        }
    }

    public void SetUpRequirements(QuestData_SO questData)
    {
        foreach (Transform item in requirementsTransform)
            Destroy(item.gameObject);

        foreach (var req in questData.questRequirements)
        {
            var newReq = Instantiate(questRequirement, requirementsTransform);
            newReq.SetUpRequirements(req.name, req.requiredAmount, req.currentAmount);
        }
    }

    public void SetUpRewardItem(ItemData_SO itemData, int amount)
    {
        var item = Instantiate(rewardUI, rewardsTransform);
        item.SetUpItemUI(itemData, amount);
    }
}
