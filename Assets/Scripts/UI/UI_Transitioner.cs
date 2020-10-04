using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Transitioner : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    public Color32 defaultColor;
    public Color32 hoverColor;
    public Color32 selectedColor;

    private Image image;

    // Start is called before the first frame update
    void Awake()
    {
        hoverColor = Color.grey;
        selectedColor = Color.red;

        image = GetComponent<Image>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        image.color = selectedColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        defaultColor = image.color;
        image.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = defaultColor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        image.color = hoverColor;
    }
}
