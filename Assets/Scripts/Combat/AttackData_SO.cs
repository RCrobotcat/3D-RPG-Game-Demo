using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack Data", menuName = "Combat/Data")]
public class AttackData_SO : ScriptableObject
{
    public float attackRange;
    public float skillRange; // distant attack range
    public float coolDown;
    public int minDamage;
    public int maxDamage;

    // Critical Hit
    public float criticalMultiplier;
    public float criticalChance;

    public void ApplyWeaponData(AttackData_SO weapon)
    {
        attackRange = weapon.attackRange;
        skillRange = weapon.skillRange;
        coolDown = weapon.coolDown;
        minDamage = weapon.minDamage;
        maxDamage = weapon.maxDamage;
        criticalMultiplier = weapon.criticalMultiplier;
        criticalChance = weapon.criticalChance;
    }
}
