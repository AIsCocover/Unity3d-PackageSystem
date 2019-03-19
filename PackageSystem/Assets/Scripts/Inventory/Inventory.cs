using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Inventory : MonoBehaviour {
    // 这里是总面板，对于多面板可以使用多个面板类去管理slots，而该类用于管理全部物品。
    // Inventory类将对所有物品的插入、删除、更新进行管理。

    // 单例
    #region Singleton
    public static Inventory instance;
    private void Awake()
    {
        if(instance != null)
        {
            Debug.Log("Instance[Inventory] is exised!");
            return;
        }

        instance = this;
    }
    #endregion

    public int inventorySize = 35;           // Inventroy的容量
    public Item[] items;                 // items, 索引对应item所在的slot编号

    private void Start()
    {
        items = new Item[inventorySize];
    }

    // 添加Item
    public void AddItem(Item item)
    {
        // 默认的物品添加方式为找到背包中首个空位置执行添加。
        for(int i=0; i<inventorySize; i++)
        {
            if (items[i] == null)
            {
                items[i] = item;
                break;
            }
        }

        // 如果找不到空位置，说明背包已满，添加失败。
    }

    // 交换两个Item的位置
    public void SwitchItem(int prevID, int nextID)
    {
        //Debug.Log("prevID: " + prevID + " nextID: " + nextID);
        if (prevID > items.Length ||
            nextID > items.Length ||
            prevID == nextID)
            return;
        // C#中的变量可以分为值类型和引用类型以及指针类型（指针类型不在讨论范围之内）
        // 当使用Item tmpItem = slots[slotID].item;时，得到的tmpItem仍然是指向slots[slotID].item所指向的地址，因此需要实现深拷贝来构建tmpItem。
        // 另外，在ScriptableObject中，由于Serialization的原因，不能使用基类引用来存储子类对象。如何解决？？？ -> 使用反射....
        // C#实现深克隆可以有3种方法：反射、序列化以及表达式树 www.cnblogs.com/Isgsanxiao/p/8205096.html
        // 在InventoryTool工具类中使用序列化/反射实现深克隆
        //Item tmpItem = InventoryTool.DeepCopy(items[nextID]);
        //Item tmpItem = (Item)InventoryTool.DeepCopyByReflection(items[nextID]);

        // 直接用变量临时引用就好了，想那么多？。。
        Item tmpItem = items[nextID];
        items[nextID] = items[prevID];
        items[prevID] = tmpItem;
    }

    // 普通的排序（整理）：将所有物品尽可能地往前移
    public void Sort()
    {
        int head = 0;
        int tail = 0;
        bool endFlag = false;  // 若一趟内tail没有找到物品，则说明整理完毕，退出循环
        while(!endFlag)
        {
            // head向右寻找空位，找到就停下
            while (head < inventorySize && items[head] != null) head++;

            // tail从head的位置出发寻找物品，找到就停下
            tail = head + 1;
            while (tail < inventorySize && items[tail] == null) tail++;

            // 交换head和tail物品所在的位置
            if (tail >= inventorySize) endFlag = true;
            else SwitchItem(head, tail);
        }
    }

    // 清空Items
    public void Clear()
    {
        for(int i=0; i<items.Length; i++)
        {
            items[i] = null;
        }
    }
}
