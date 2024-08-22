using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Data", menuName = "CharacterStatus/Data")]
public class CharacterData_SO : ScriptableObject
{
    [Header("Status Info")]
    public int maxHealth;
    public int currentHealth;
    public int baseDefence;
    public int currentDefence;

    [Header("Kill Info for Killing Enemies")]
    public int killPoint;

    [Header("Level Info")]
    public int currentLevel;
    public int maxLevel;
    public int baseExp;
    public int currentExp;
    public float levelBuff;
    public float levelMultiplier
    {
        get { return 1 + (currentLevel - 1) * levelBuff; }
    }

    public void UpdateExp(int point)
    {
        currentExp += point;
        if (currentExp >= baseExp)
            LevelUp();
    }

    private void LevelUp()
    {
        currentLevel = Mathf.Clamp(currentLevel + 1, 0, maxLevel);
        baseExp += (int)(baseExp * levelMultiplier);

        maxHealth = (int)(maxHealth * levelMultiplier);
        currentHealth = maxHealth;

        baseDefence = (int)(baseDefence * levelMultiplier + 1);
        currentDefence = baseDefence;

        Debug.Log("Level Up!" + currentLevel + "Max Health: " + maxHealth
            + "Base Defence: " + baseDefence + "Base Exp: " + baseExp);
        // TODO: Level Up Effect(UI Animation)
    }
}
