using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Useable Item",menuName = "Inventory/Useable Item Date")]
public class UseableItemDate_SO : ScriptableObject
{
    public int healthPoint;
    public int buffAttack;
    public float buffMultiplier;
    public float buffCriticalChance;
}
