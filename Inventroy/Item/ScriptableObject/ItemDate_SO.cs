using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { Useable,Weapon,Armor}
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item Date")]

public class ItemDate_SO : ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    public Sprite itemIcon;
    public int itemAmount;
    public bool stackable;

    [TextArea]
    public string description = "";

    [Header("Useable Item")]
    public UseableItemDate_SO useableDate;

    [Header("Weapon")]
    public AttackDate_SO weaponDate;
    public GameObject WeaponPerfab;
    public AnimatorOverrideController weaponAnimator;

    [Header("Armor")]
    public ArmorDate_SO armorDate;
    public GameObject armorPerfab;

}
