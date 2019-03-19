using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment")]
public class Equipment : Item {

    public EquipmentType equipType;

    public int damageModifier;
    public int armorModifier;

    public override void Use()
    {
        base.Use();
    }

    public override string ToString()
    {
        string str = (damageModifier == 0 ? "" : "DamageModifier: " + damageModifier + "\n") +
            (armorModifier == 0 ? "" : "ArmorModifier: " + armorModifier);
        return str;
    }
}

public enum EquipmentType
{
    Head, Chest, Legs, Weapon, Shield, Fest
}