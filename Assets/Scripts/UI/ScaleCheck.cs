using UnityEngine.UI;
using UnityEngine;

public class ScaleCheck : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);       
    }

}
