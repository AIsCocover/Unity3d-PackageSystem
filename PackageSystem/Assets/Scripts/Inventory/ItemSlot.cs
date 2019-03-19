using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ItemSlot对应一个item_slot实体
public class ItemSlot : MonoBehaviour {

    public Image icon;                  // Item的图标
    public Button closeButton;         // 丢弃按钮

    public Item item;                   // Item引用
    public int slotID;                   // slot编号 由InventoryUITool类来批量初始化

    private void Start()
    {
        // 如果有实现状态保存，这里可以实现初始化函数来初始化slot

        this.icon.gameObject.SetActive(false);
        this.closeButton.gameObject.SetActive(false);

        this.item = null;
    }

    // 插入item
    public void InsertItem(Item item)
    {
        if (item == null) return;
        this.item = item;
        this.icon.sprite = item.icon;
        this.icon.gameObject.SetActive(true);
    }

    // 清空槽
    public void ClearSlot()
    {
        this.icon.gameObject.SetActive(false);
        this.closeButton.gameObject.SetActive(false);

        if (this.item != null)
            this.item = null;
    }
}
