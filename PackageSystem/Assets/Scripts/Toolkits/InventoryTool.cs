using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class InventoryTool : MonoBehaviour {

    public GameObject slotPrefab;
    public int slotCount = 35;

    // 批量生成item_slot
    [ContextMenu("Generate Slots")]
    public void GenerateSlots()
    {
        for(int i=0; i<slotCount; i++)
        {
            GameObject obj = Instantiate(slotPrefab, transform);
            obj.name = "item_slot" + "_" + i;

            ItemSlot itemSlot = obj.GetComponent<ItemSlot>();
            if(itemSlot != null)
            {
                itemSlot.slotID = i;
            } else
            {
                Debug.Log("Missing component[ItemSlot]. Please check your prefab again!");
            }
        }

        //DestroyImmediate(this);
    }

    // 序列化实现深拷贝
    public static T DeepCopy<T> (T source)
    {
        // 判断是否可序列化
        if(typeof(T).IsSerializable == false)
        {
            Debug.Log("Type: " + typeof(T) + " can not serialize.");
            return default(T);
        }
        // 判空
        if(System.Object.ReferenceEquals(source, null))
        {
            return default(T);
        }

        IFormatter formatter = new BinaryFormatter();
        Stream stream = new MemoryStream();
        using(stream)
        {
            formatter.Serialize(stream, source);
            // Stream.Seek(offset, origin) 将以origin为参考点，将指针移到offset位置。
            stream.Seek(0, SeekOrigin.Begin);
            return (T)formatter.Deserialize(stream);
        }
    }

    // 反射实现深拷贝
    public static object DeepCopyByReflection(object obj)
    {
        if (obj == null)
            return null;

        System.Object targetObj;
        Type targetType = obj.GetType();
    
        // 对于值类型，直接使用=即可
        if (targetType.IsValueType == true)
            targetObj = obj;
        else {
            // 对于引用类型
            // 根据targetType创建引用对象
            targetObj = System.Activator.CreateInstance(targetType);
            // 使用反射获取targetType下所有的成员
            System.Reflection.MemberInfo[] memberInfos = obj.GetType().GetMembers();

            // 遍历所有的成员
            foreach(System.Reflection.MemberInfo member in memberInfos) {
                // 如果成员为字段类型：获取obj中该字段的值。
                if(member.MemberType == System.Reflection.MemberTypes.Field) {
                    System.Reflection.FieldInfo fieldInfo = (System.Reflection.FieldInfo)member;
                    System.Object fieldValue = fieldInfo.GetValue(obj);

                    //如果该值可直接Clone，则直接Clone；否则，递归调用DeepCopyByReflection(该值)。
                    if(fieldValue is ICloneable) {
                        fieldInfo.SetValue(targetObj, (fieldValue as ICloneable).Clone());
                    } else {
                        fieldInfo.SetValue(targetObj, DeepCopyByReflection(fieldValue));
                    }
                }
                // 如果成员为属性类型：获取obj中该属性的值。
                else if (member.MemberType == System.Reflection.MemberTypes.Property) {
                    System.Reflection.PropertyInfo propertyInfo = (System.Reflection.PropertyInfo)member;

                    // GetSetMethod(nonPublic) nonPublic means: sIndicates whether the accessor should be returned if it is non-public. true if a non-public accessor is to be returned; otherwise, false.
                    System.Reflection.MethodInfo methodInfo = propertyInfo.GetSetMethod(false);

                    if (methodInfo != null) {
                        try {
                            // 如果该值可直接CLone，则直接Clone；否则，递归调用DeepCopyByReflection(该值)。
                            object propertyValue = propertyInfo.GetValue(obj, null);
                            if(propertyValue is ICloneable)
                            {
                                propertyInfo.SetValue(targetObj, (propertyValue as ICloneable).Clone(), null);
                            } else {
                                propertyInfo.SetValue(targetObj, DeepCopyByReflection(propertyValue), null);
                            }
                        } catch (Exception e) {
                            // some thing except.
                            Debug.Log(e.Message);
                        }
                    }
                }
            }
        }

        return targetObj;
    }
}
