using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject {
    new public string name = "New Item";
    public string description = "";
    public Sprite icon = null;
    public ItemType type;

    public virtual void Use()
    {
        Debug.Log("Item: " + name + " used.");
    }
}

public enum ItemType
{
    Equipment,
    Potion
}