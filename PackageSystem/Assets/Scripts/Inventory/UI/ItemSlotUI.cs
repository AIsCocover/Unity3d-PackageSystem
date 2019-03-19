using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/**
    使用EventSystems中的IPointerHandler来检测鼠标是否移动到了一个GameObject上。
    如果无法检测到当前的物体：
        请检查当前物体是否带有开启了Raycast Target的UI组件；
        请检查该物体的子物体下是否有别的UI开启了Raycast Target。

    在ItemSlotUI类依附的物体itemslot中，使用了一个raycast_image作为Raycast Target的触发器。

    将在InventoryUI中监听各种事件。
     */
public class ItemSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler {

    // 静态static 所有ItemSlotUI对象共享
    public static Action<Transform> OnPointerEnterAction;
    public static Action OnPointerExitAction;
    public static Action<ItemSlot> OnPointerDownAction;
    public static Action<ItemSlot,ItemSlot> OnPointerUpAction;


    // 当鼠标移动到一个UI上时会触发，被检测的UI需要勾选Raycast Target属性
    public void OnPointerEnter(PointerEventData eventData)
    {
        // 该Debug语句将输出"raycast_image"
        //Debug.Log("OnPointerEnter: " + eventData.pointerEnter.name);

        // 启动监听器
        // 当然，这里可以直接调用InventroyUI的方法，但只适用于需要需要调用的方法较少的情况。使用委托可以更好的处理这个问题。
        if (OnPointerEnterAction != null)
            OnPointerEnterAction(transform);
    }

    // 当鼠标离开一个UI时会触发，被检测的UI需要勾选Raycast Target属性
    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("OnPointerExit: " + gameObject.name);

        // 启动监听器
        // 当然，这里可以直接调用InventroyUI的方法，但只适用于需要需要调用的方法较少的情况。使用委托可以更好的处理这个问题。
        if (OnPointerExitAction != null)
            OnPointerExitAction();
    }

    // 当在一个UI上按下鼠标时会触发，被检测的UI需要勾选Raycast Target属性
    public void OnPointerDown(PointerEventData eventData)
    {
        // 无法获取pointerPress，因而换用pointerCurrentRaycast.gameObject
        //Debug.Log("OnPointerDown: " + eventData.pointerPress.name);
        //Debug.Log("OnPointerDown: " + eventData.pointerCurrentRaycast.gameObject.name);

        // 接收射线检测的是item_slot下的raycast_image
        ItemSlot itemSlot = eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<ItemSlot>();

        if (OnPointerDownAction != null)
            OnPointerDownAction(itemSlot);
    }

    // 当在一个UI上松开鼠标时会触发，被检测的UI需要勾选Raycast Target属性
    public void OnPointerUp(PointerEventData eventData)
    {
        ItemSlot currentSlot = transform.GetComponent<ItemSlot>();
        ItemSlot nextSlot = eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<ItemSlot>();

        if (OnPointerUpAction != null)
            OnPointerUpAction(currentSlot, nextSlot);
    }
}
