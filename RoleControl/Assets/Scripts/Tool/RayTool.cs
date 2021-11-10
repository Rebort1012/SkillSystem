using System.Collections;
using System.Collections.Generic;
//using Newtonsoft.Json.Converters;
using UnityEngine;
using UnityEngine.EventSystems;

public static class RayTool
{
    public static string RaycastUI()
    {
        if (EventSystem.current == null)
            return null;
        //鼠标点击事件
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        //设置鼠标位置
        pointerEventData.position = Input.mousePosition;
        //射线检测返回结果
        List<RaycastResult> results = new List<RaycastResult>();
        //检测UI
        //graphicRaycaster.Raycast(pointerEventData, results);
        EventSystem.current.RaycastAll(pointerEventData, results);

        if (results.Count > 0)
            return results[0].gameObject.name;
        else
            return null;
    }
}
