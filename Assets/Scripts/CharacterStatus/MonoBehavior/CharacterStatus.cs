using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatus : MonoBehaviour
{
    public event Action<int, int> updateHealthBarOnAttack;
    public CharacterData_SO templateData;
    [HideInInspector] public CharacterData_SO characterData;

    public AttackData_SO attackData;
    private AttackData_SO baseAttackData;
    private RuntimeAnimatorController baseAnimator;

    public bool isInvincible;

    [Header("Weapon")]
    public Transform WeaponSlot;

    [Header("Armor")]
    public Transform ArmorSlot;

    [HideInInspector]
    public bool isCritical;

    private void Awake()
    {
        // duplicate the template data to the character data
        if (templateData != null)
            characterData = Instantiate(templateData);

        baseAttackData = Instantiate(attackData);

        baseAnimator = GetComponent<Animator>().runtimeAnimatorController;
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
        if (isInvincible) return;
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
        if (isInvincible) return;
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
            // Debug.Log("Critical Hit: " + coreDamage);
        }

        return (int)coreDamage;
    }

    #endregion

    #region Equip Weapon
    public void SwitchWeapon(ItemData_SO weapon)
    {
        UnEquipWeapon();
        EquipWeapon(weapon);
    }

    public void EquipWeapon(ItemData_SO weapon)
    {
        if (weapon.WeaponPrefab != null)
            Instantiate(weapon.WeaponPrefab, WeaponSlot);

        // Add weapon stats to the player
        attackData.ApplyWeaponData(weapon.WeaponData);

        GetComponent<Animator>().runtimeAnimatorController = weapon.WeaponAnimator;

        // InventoryManager.Instance.UpdateStatusText(maxHealth, attackData.maxDamage, attackData.minDamage);
    }

    public void UnEquipWeapon()
    {
        if (WeaponSlot.childCount != 0)
        {
            for (int i = 0; i < WeaponSlot.childCount; i++)
            {
                Destroy(WeaponSlot.GetChild(i).gameObject);
            }
        }
        attackData.ApplyWeaponData(baseAttackData);

        GetComponent<Animator>().runtimeAnimatorController = baseAnimator;
    }
    #endregion

    #region Equip Armor
    public void SwitchArmor(ItemData_SO armor)
    {
        UnEquipArmor();
        EquipArmor(armor);
    }

    public void EquipArmor(ItemData_SO armor)
    {
        if (armor.ArmorPrefab != null)
            Instantiate(armor.ArmorPrefab, ArmorSlot);

        characterData.ApplyDefenceData(armor.ArmorDefenceData);

        GetComponent<Animator>().runtimeAnimatorController = armor.ArmorAnimator;
    }
    public void UnEquipArmor()
    {
        if (ArmorSlot.childCount != 0)
        {
            for (int i = 0; i < WeaponSlot.childCount; i++)
            {
                Destroy(ArmorSlot.GetChild(i).gameObject);
            }
        }

        characterData.baseDefence = templateData.baseDefence;
        characterData.currentDefence = characterData.baseDefence;

        GetComponent<Animator>().runtimeAnimatorController = baseAnimator;
    }
    #endregion

    #region Apply Data Change
    public void ApplyHealth(int amount)
    {
        if (currentHealth + amount <= maxHealth)
            currentHealth += amount;
        else currentHealth = maxHealth;
    }
    #endregion
}
