using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    //創建一個可以找到任何Child的擴充功能
    public static T FindAnyChild<T>(this Transform trans,string name) where T : Component
    {
        for (int n = 0; n < trans.childCount; n++)
        {
            if (trans.GetChild(n).childCount > 0)
            {
                var child = trans.GetChild(n).FindAnyChild<Transform>(name);
                if (child != null)
                    return child.GetComponent<T>();
            }

            if (trans.GetChild(n).name == name)
            {
                return trans.GetChild(n).GetComponent<T>();
            }
        }
        return default;
    }
}
