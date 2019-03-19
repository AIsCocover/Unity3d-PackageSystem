using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class InventoryUI : MonoBehaviour {

    public static Action OnInventoryUIUpdateAction;

    public Transform slotsRoot;             // 存放所有item_slot的根结点
    public TooltipUI tooltipPanel;         // 提示面板
    public Item[] itemPrefabs;            // 可用Item列表

    private List<ItemSlot> slots;            // slots，用于存储slot
    private bool isShowing = false;
    private bool isDraging = false;

    private void Start()
    {
        slots = new List<ItemSlot>();

        // 初始化slots
        if (slotsRoot != null)
        {
            int slotsCount = slotsRoot.childCount;
            for (int i = 0; i < slotsCount; i++)
            {
                ItemSlot itemSlot = slotsRoot.GetChild(i).GetComponent<ItemSlot>();
                slots.Add(itemSlot);
            }
        }

        // 添加监听
        OnInventoryUIUpdateAction += InventoryUI_OnUpdate;

        ItemSlotUI.OnPointerEnterAction += ItemSlotUI_OnEnter;
        ItemSlotUI.OnPointerExitAction += ItemSlotUI_OnExit;
        ItemSlotUI.OnPointerDownAction += ItemSlotUI_OnDown;
        ItemSlotUI.OnPointerUpAction += ItemSlotUI_OnUp;
    }

    private void Update()
    {
        // tooltipPanel的显示
        if (isShowing)
        {
            // 将鼠标所在的位置转换为localPosition
            Vector2 rectPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                GameObject.Find("PackageUI").transform as RectTransform,
                Input.mousePosition,
                null,
                out rectPos
                );
            tooltipPanel.gameObject.transform.localPosition = rectPos;

            tooltipPanel.Display();
        }

        // 图标拖拽
        if (isDraging)
        {
            // 实现思路：Copy一份icon，让icon跟随鼠标移动。

        }
    }
    
    // ItemSlotUI中鼠标事件的监听
    #region Listener: ItemSlotUI
    // 监听ItemSlotUI的OnPointerEnter事件
    private void ItemSlotUI_OnEnter(Transform itemSlot)
    {
        Item item = itemSlot.GetComponent<ItemSlot>().item;
        if (item != null)
        {
            // 直接对RectTransform赋值会产生一个问题：tooltipPanel会发生闪烁，原因在于tolltipPanel显示时将会挡住射线检测->监听到PointerExit->tooltipPanel消失->检测到射线->监听到PointerEnter->tooltipPanel显示->一直循环下去。
            // 解决方法：让tooltipPanel随鼠标移动，同时将tooltipPanel的pivot向左上方偏移一段距离(移出tooltipPanel的整体范围，即pivot处于tooltipPanel之外)，让tooltipPanel永久处于鼠标尾部右下方，防止鼠标碰到tooltipPanel。
            // 注意：Scene面板上方Toggle Tool Handle Position设置为Pivot时才可拖动tooltipPanel的pivot。
            //tooltipPanel.gameObject.GetComponent<RectTransform>().anchoredPosition = itemSlot.gameObject.GetComponent<RectTransform>().anchoredPosition;

            string desc = GetItemDescription(item);
            tooltipPanel.UpdateContent(desc);
            isShowing = true;
        }
    }

    // 监听ItemSlotUI的OnPointerExit事件
    private void ItemSlotUI_OnExit()
    {
        tooltipPanel.UpdateContent("");
        tooltipPanel.Hide();
        isShowing = false;
    }

    // 监听ItemSlotUI的OnPointerDown事件
    private void ItemSlotUI_OnDown(ItemSlot itemSlot)
    {
        // 如果没有点击到itemSlot或者itemSlot没有物体，则直接返回
        if (itemSlot == null || itemSlot.item == null) return;

        // 将鼠标样式设置为itemSlot中item对应的icon
        // 或者是让当前icon变得暗淡，然后copy一个icon跟随鼠标移动
        // 为了避免icon妨碍到OnPointerUp事件的触发，需要将icon的Raycast Target设为false

        isDraging = true;
    }

    // 监听ItemSlotUI的OnPointerUp事件
    private void ItemSlotUI_OnUp(ItemSlot currentSlot, ItemSlot nextSlot)
    {
        // 你无法从空槽里面拖拽出任何东西。
        if (currentSlot.item == null) return;

        // 松开鼠标后，如果检测到slot，则运行逻辑：
        if(nextSlot != null)
        {
            int currentID = currentSlot.slotID;
            int nextID = nextSlot.slotID;
            Inventory.instance.SwitchItem(currentID, nextID);
        }

        if (OnInventoryUIUpdateAction != null)
            OnInventoryUIUpdateAction();

        isDraging = false;
    }

    private void InventoryUI_OnUpdate()
    {
        Item[] items = Inventory.instance.items;
        for(int i=0; i<slots.Count; i++)
        {
            slots[i].ClearSlot();
            if (items[i] != null)
                slots[i].InsertItem(items[i]);
        }
    }

    // 辅助方法，用于格式化关于Item的文本
    private string GetItemDescription(Item item)
    {
        if (item == null) return "";

        // 对string进行频繁连接使用StringBuilder效率更高。
        StringBuilder str = new StringBuilder();
        str.AppendFormat("<color=red>{0}</color>\n\n", item.name);
        /*
        switch (item.type)
        {
            case ItemType.Equipment:
                Equipment equipment = (Equipment)item;
                str.Append(equipment.ToString());
                break;
            case ItemType.Potion:
                Potion potion = (Potion)item;
                str.Append(potion.ToString());
                break;
            default:
                break;
        }*/

        str.AppendFormat("\n\n<color=blue>{0}</color>", item.ToString());
        str.AppendFormat("\n\n<color=white><size=10>{0}</size></color>", item.description);

        return str.ToString();
    }
    #endregion

    // 鼠标事件的监听，由U3D面板的Button UI直接调用
    #region OnButtonClickEvent
    // 从可用items中随机选取单个item加入inventory中。
    public void OnClick_AddItemRandom()
    {
        int index = UnityEngine.Random.Range(0, itemPrefabs.Length);
        Inventory.instance.AddItem(itemPrefabs[index]);

        if (OnInventoryUIUpdateAction != null)
            OnInventoryUIUpdateAction();
    }

    // 普通的排序（整理）：将所有物品尽可能地往前移
    public void OnClick_Sort()
    {
        Inventory.instance.Sort();

        if (OnInventoryUIUpdateAction != null)
            OnInventoryUIUpdateAction();
    }

    public void OnClick_Clear()
    {
        Inventory.instance.Clear();
        if (OnInventoryUIUpdateAction != null)
            OnInventoryUIUpdateAction();
    }
    #endregion
}
