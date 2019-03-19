using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
    Tooltip用于显示Item的Description信息。

    实现原理：
        通过使用Content Size Fitter组件来控制Tooltip边框自适应Description信息的长度。
        因此必须保持tooltip的Text组件中的Text和content中的Text的内容一致！！！

    注意：
         这里设置仅自适应长度，宽度固定
*/
public class TooltipUI : MonoBehaviour {
    public Text fitterText;
    public Text contentText;

    // 更新tooltip显示内容时要同时更新fiterText中的内容，保持两者内容一致。
    public void UpdateContent(string desc)
    {
        contentText.text = desc;
        fitterText.text = contentText.text;
    }

    public void Display()
    { gameObject.SetActive(true); }

    public void Hide()
    { gameObject.SetActive(false); }
}
