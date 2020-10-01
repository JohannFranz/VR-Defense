using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TD_Utility
{
    public class Utility
    {
        public static GameObject GetChildByTag(GameObject parent, string tag)
        {
            Transform childTrans = parent.transform;
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                GameObject child = childTrans.GetChild(i).gameObject;
                if (child != null && child.CompareTag(tag))
                    return child;

                GameObject subChild = GetChildByTag(child, tag);
                if (subChild != null && subChild.CompareTag(tag))
                    return subChild;
            }
            return null;
        }

        public static bool IsNumberInList(int[] list, int number)
        {
            for (int i = 0; i < list.Length; i++)
            {
                if (list[i] == number)
                    return true;
            }

            return false;
        }
    }
}

