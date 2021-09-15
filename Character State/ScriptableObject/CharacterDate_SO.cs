using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Date",menuName ="Character Date/Date")]
public class CharacterDate_SO : ScriptableObject
{
    [Header("State Info")]
    public int maxHealth;
    public int currentHealth;
    public int baseDefence;
    public int currentDefence;

    [Header("Kill")]
    public int killPoint;

    [Header("Level")]
    public int currentLevel;
    public int maxLevel;
    public int baseExp;
    public int currentExp;
    public float levelBuff;

    public float LevelMultipler
    {
        get { return 1 + (currentLevel - 1) * levelBuff; }
    }

    public void UpdateExp(int point)
    {
        currentExp += point;

        if(currentExp >= baseExp)
        {
            LevelUp(); 
        }
    }

    private void LevelUp()
    {
        //todo:to do method
        currentLevel = Mathf.Clamp(currentLevel + 1, 0, maxLevel);
        baseExp += (int)(baseExp * LevelMultipler);

        maxHealth = (int)(maxHealth * LevelMultipler);
        currentHealth = maxHealth;
    }

    public void ApplyArmorDate(ArmorDate_SO armor)
    {
        baseDefence = armor.extraDefence + baseDefence;
        currentDefence = armor.extraDefence + currentDefence;
    }

    public void ApplyRandomArmorDate(ArmorDate_SO armor)
    {
        System.Random randomDefence = new System.Random();
        int defence = randomDefence.Next(1, 6);

        baseDefence = defence + baseDefence;
        currentDefence = defence + currentDefence;
    }
}
