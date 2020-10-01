using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Code from https://answers.unity.com/questions/1333760/scrollview-how-to-stop-scrolling-if-there-are-no-m.html
public class ScrollManager : MonoBehaviour
{
    public RectTransform cont;
    int maxScrollHeight;

    // Start is called before the first frame update
    void Start()
    {
        GameObject content = transform.Find("Viewport").GetChild(0).gameObject;
        UnityEngine.UI.VerticalLayoutGroup group = content.GetComponent<UnityEngine.UI.VerticalLayoutGroup>();
        int padding = group.padding.top + group.padding.bottom;
        maxScrollHeight = Constants.scrollButtonHeight * Constants.countWorlds + padding;
    }

    // Update is called once per frame
    void Update()
    {
        if (cont.offsetMax.y < 0)
        { //It seems that is checking for less than 0, but the syntax is weird
            cont.offsetMax = new Vector2(); //Sets its value back.
            cont.offsetMin = new Vector2(); //Sets its value back.
        }

        if (cont.offsetMax.y > maxScrollHeight - Constants.scrollButtonHeight)
        { // Checks the values
            cont.offsetMax = new Vector2(0, maxScrollHeight - Constants.scrollButtonHeight); // Set its value back
            cont.offsetMin = new Vector2(); //Depending on what values you set on your scrollview, you                                              might want to change this, but my one didn't need it.
        }
    }
}
