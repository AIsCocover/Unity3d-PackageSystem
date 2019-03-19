using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Potion", menuName = "Inventory/Potion")]
public class Potion : Item {

    public PotionType potionType;

    public int hpRegen;
    public int mpRegen;

    public override void Use()
    {
        base.Use();
        string repStr = (hpRegen == 0 ? "" : "HP Regen: " + hpRegen) + "\n" + (mpRegen == 0 ? "" : "MP Regen: " + mpRegen);
        Debug.Log(repStr);
    }

    public override string ToString()
    {
        string str = (hpRegen == 0 ? "" : "HP Regen: " + hpRegen) + "\n" + (mpRegen == 0 ? "" : "MP Regen: " + mpRegen);
        return str;
    }
}

public enum PotionType
{
    HealthPotion,
    ManaPotion,
    CompositePotion
}