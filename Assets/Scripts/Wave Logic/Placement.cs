using UnityEngine;

public class Placement : MonoBehaviour
{
    public PlacementData placementData;

    private GameObject placedMercenary;
    private Renderer placementRenderer;
    private Color unplacedColor;
    private Color placedColor;

    private bool isAlreadyOccupied;

    // Start is called before the first frame update
    void Start()
    {
        isAlreadyOccupied = false;

        unplacedColor = placementData.unplacedColor;
        placedColor = placementData.placedColor;
        placementRenderer = GetComponent<Renderer>();
        placementRenderer.material.color = unplacedColor;
    }

    void Update()
    {
        if (placedMercenary == null || placedMercenary.activeSelf == false)
            isAlreadyOccupied = false;
    }

    public bool IsAlreadyOccupied()
    {
        return isAlreadyOccupied;
    }

    public void Occupy(GameObject mercenary)
    {
        placedMercenary = mercenary;
        isAlreadyOccupied = true;
    }
}
