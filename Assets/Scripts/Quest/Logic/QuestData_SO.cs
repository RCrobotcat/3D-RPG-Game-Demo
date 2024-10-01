using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest/Quest Data")]
public class QuestData_SO : ScriptableObject
{
    [System.Serializable]
    public class QuestRequirement
    {
        public string name;
        public int requiredAmount;
        public int currentAmount;
    }

    public string questName;
    [TextArea]
    public string description;

    public bool isStarted; // is the quest started
    public bool isCompleted; // is the quest completed
    public bool isFinished; // is the quest finished(after completed and received rewards)

    public List<QuestRequirement> questRequirements = new List<QuestRequirement>();
    public List<InventoryItem> questRewards = new List<InventoryItem>();

    public void CheckTaskProgress()
    {
        var finishedRequires = questRequirements.Where(r => r.currentAmount >= r.requiredAmount);
        isCompleted = finishedRequires.Count() == questRequirements.Count;

        if (isCompleted)
            Debug.Log("Quest Completed!");
    }
}
