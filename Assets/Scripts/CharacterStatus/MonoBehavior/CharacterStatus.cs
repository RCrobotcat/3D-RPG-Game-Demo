using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatus : MonoBehaviour
{
    public event Action<int, int> updateHealthBarOnAttack;
    public CharacterData_SO templateData;
    public CharacterData_SO characterData;
    public AttackData_SO attackData;

    [HideInInspector]
    public bool isCritical;

    private void Awake()
    {
        // duplicate the template data to the character data
        if (templateData != null)
            characterData = Instantiate(templateData);
    }

    #region Read From ScriptableObject Data_SO
    // get: means that you can only get the value of the variable;(read only) -> characterData.maxHealth
    // set: means that you can only set the value of the variable;(write only) -> characterData.maxHealth = 100;
    public int maxHealth
    {
        get
        {
            if (characterData != null)
                return characterData.maxHealth;
            else return 0;
        }
        set
        {
            characterData.maxHealth = value;
        }
    }

    public int currentHealth
    {
        get
        {
            if (characterData != null)
                return characterData.currentHealth;
            else return 0;
        }
        set
        {
            characterData.currentHealth = value;
        }
    }

    public int baseDefence
    {
        get
        {
            if (characterData != null)
                return characterData.baseDefence;
            else return 0;
        }
        set
        {
            characterData.baseDefence = value;
        }
    }

    public int currentDefence
    {
        get
        {
            if (characterData != null)
                return characterData.currentDefence;
            else return 0;
        }
        set
        {
            characterData.currentDefence = value;
        }
    }
    #endregion

    #region Character Combat

    public void TakeDamage(CharacterStatus attacker, CharacterStatus definer)
    {
        int damage = Mathf.Max(attacker.currentDamage() - definer.currentDefence, 0);
        currentHealth = Mathf.Max(currentHealth - damage, 0);

        if (attacker.isCritical)
            definer.GetComponent<Animator>().SetTrigger("Hit");

        //if (updateHealthBarOnAttack != null), then invoke it.
        updateHealthBarOnAttack?.Invoke(currentHealth, maxHealth);
        // Update Level by Exp
        if (currentHealth <= 0)
            attacker.characterData.UpdateExp(characterData.killPoint);
    }

    // overload
    public void TakeDamage(int damage, CharacterStatus definer)
    {
        int currentDamage = Mathf.Max(damage - definer.currentDefence, 0);
        currentHealth = Mathf.Max(currentHealth - currentDamage, 0);
        updateHealthBarOnAttack?.Invoke(currentHealth, maxHealth);
        if (currentHealth <= 0)
        {
            GameManager.Instance.playerStatus.characterData.UpdateExp(characterData.killPoint);
        }
    }

    private int currentDamage()
    {
        float coreDamage = UnityEngine.Random.Range(attackData.minDamage, attackData.maxDamage);

        if (isCritical)
        {
            coreDamage *= attackData.criticalMultiplier;
            Debug.Log("Critical Hit: " + coreDamage);
        }

        return (int)coreDamage;
    }

    #endregion
}
