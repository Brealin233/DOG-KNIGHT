using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CharacterStates : MonoBehaviour
{
    public event Action<int, int> UpDateHealthBarOnAttack;

    public CharacterDate_SO templateDate;

    public CharacterDate_SO characterDate;

    public AttackDate_SO attackDate;

    public ArmorDate_SO armorDate;

    public AttackDate_SO baseAttackDate;

    private RuntimeAnimatorController baseAnimator;

    public int damageShow;

    [Header("Weapon")]
    public Transform commonWeaponSlot;
    public Transform caidaoSlot;
    public Transform dakandaoSlot;

    [Header("Armor")]
    public Transform commonArmorSlot;

    [Header("Weapon Stats")]
    public bool xiaolifeidao;
    public bool isCritical;

    //[Header("Weapon Effect")]
    //public  bool isWeaponTrail;
    

    void Awake()
    {
        if (templateDate != null)
            characterDate = Instantiate(templateDate);

        attackDate = Instantiate(baseAttackDate);
        baseAnimator = GetComponent<Animator>().runtimeAnimatorController;
    }

    #region Read Form Date_SO
    public int MaxHealth
    {
        get
        {
            if (characterDate != null)
                return characterDate.maxHealth;
            else
                return 0;
        }
        set
        {
            characterDate.maxHealth = value;
        }
    }

    public int CurrentHealth
    {
        get
        {
            if (characterDate != null)
                return characterDate.currentHealth;
            else
                return 0;
        }
        set
        {
            characterDate.currentHealth = value;
        }
    }

    public int BaseDenfense
    {
        get
        {
            if (characterDate != null)
                return characterDate.baseDefence;
            else
                return 0;
        }
        set
        {
            characterDate.baseDefence = value;
        }
    }

    public int CurrentDenfense
    {
        get
        {
            if (characterDate != null)
                return characterDate.currentDefence;
            else
                return 0;
        }
        set
        {
            characterDate.currentDefence = value;
        }
    }
    #endregion

    #region Character Combat

    public void TakeDamage(CharacterStates attacker,CharacterStates defener)
    {
        int damage = Mathf.Max(attacker.CurrentDamage() - defener.CurrentDenfense,0);
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);

        attacker.damageShow = damage;

        if (attacker.isCritical)
            defener.GetComponent<Animator>().SetTrigger("Hit");

        UpDateHealthBarOnAttack?.Invoke(CurrentHealth, MaxHealth);

        if (CurrentHealth <= 0)
            attacker.characterDate.UpdateExp(characterDate.killPoint);
    }

    public void TakeDamage(int damage,CharacterStates defener)
    {
        int currentDamage = Mathf.Max(damage - defener.CurrentDenfense, 0);
        CurrentHealth = Mathf.Max(CurrentHealth - currentDamage, 0);

        damageShow = currentDamage;

        UpDateHealthBarOnAttack?.Invoke(CurrentHealth, MaxHealth);

        if (CurrentHealth <= 0)
        {
            GameManager.Instance.playStats.characterDate.UpdateExp(characterDate.killPoint);
        } 
    }

    private int CurrentDamage()
    {
        float coreDamage = UnityEngine.Random.Range(attackDate.minDamge, attackDate.maxDamge);

        if (isCritical)
            coreDamage *= attackDate.criticalMultiplier;

        return (int)coreDamage;
    }

    #endregion

    #region EquipWeapon

    public void ChangeWeapon(ItemDate_SO weapon)
    {
        UnEquipWeapon();
        EquipWeapon(weapon);
    }

    public void ChangeAromor(ItemDate_SO armor)
    {
        UnEquipArmor();
        EquipArmor(armor);
    }

    public void EquipWeapon(ItemDate_SO weapon)
    {
        if(weapon.WeaponPerfab != null)
        {
            if (weapon.WeaponPerfab.tag == "caidao")
            {
                Instantiate(weapon.WeaponPerfab, caidaoSlot);
            }
            else if(weapon.WeaponPerfab.tag == "kandao")
            {
                Instantiate(weapon.WeaponPerfab, dakandaoSlot);
            }
            else if(weapon.WeaponPerfab.tag == "changqiang")
            {
                Instantiate(weapon.WeaponPerfab, dakandaoSlot);
            }
            else if(weapon.WeaponPerfab.tag == "kexuezhanfu")
            {
                Instantiate(weapon.WeaponPerfab, dakandaoSlot);
            }
            else if (weapon.WeaponPerfab.tag == ("xiaolifeidao"))
            {
                Instantiate(weapon.WeaponPerfab, commonWeaponSlot);
                xiaolifeidao = true;
            }
            else 
                Instantiate(weapon.WeaponPerfab, commonWeaponSlot);
        }
        

        //todo:更新数据
        if (weapon.itemType == ItemType.Weapon)
        {
            attackDate.AppleWeaponDate(weapon.weaponDate);
            GetComponent<Animator>().runtimeAnimatorController = weapon.weaponAnimator;
        }
    }

    public void UnEquipWeapon()
    {
        if(commonWeaponSlot.transform.childCount != 0)
        {
            for (int i = 0; i < commonWeaponSlot.transform.childCount; i++)
            {
                Destroy(commonWeaponSlot.transform.GetChild(i).gameObject);
            }
        }

        if (caidaoSlot.transform.childCount != 0)
        {
            for (int i = 0; i < caidaoSlot.transform.childCount; i++)
            {
                Destroy(caidaoSlot.transform.GetChild(i).gameObject);
            }
        }

        xiaolifeidao = false;
        attackDate.AppleWeaponDate(baseAttackDate);
        GetComponent<Animator>().runtimeAnimatorController = baseAnimator;
    }

    public void EquipArmor(ItemDate_SO armor)
    {
        if (armor.armorPerfab != null)
        {
            Instantiate(armor.armorPerfab, commonArmorSlot);
        }

        if (armor.itemType == ItemType.Armor)
        {
            characterDate.ApplyArmorDate(armor.armorDate);
        }
    }

    public void UnEquipArmor()
    {
        if (commonArmorSlot.transform.childCount != 0)
        {
            for (int i = 0; i < commonArmorSlot.transform.childCount; i++)
            {
                Destroy(commonArmorSlot.transform.GetChild(i).gameObject);
            }
        }
        characterDate.baseDefence = 0;
        characterDate.currentDefence = 1;
    }

    #endregion

    #region Apply Date Change

    public void ApplyHealth(int amount)
    {
        if (CurrentHealth + amount <= MaxHealth)
            CurrentHealth += amount;
        else
            CurrentHealth = MaxHealth;
    }

    public void ApplyBuff(int attackBuff, float CriticalChance)
    {
        attackDate.minDamge += attackBuff;
        attackDate.maxDamge += attackBuff;
        attackDate.criticalChance += CriticalChance;
    }

    public void AppledeBuff(int attackBuff, float CriticalChance)
    {
        attackDate.minDamge -= attackBuff;
        attackDate.maxDamge -= attackBuff;
        attackDate.criticalChance -= CriticalChance;
    }

    #endregion
}
