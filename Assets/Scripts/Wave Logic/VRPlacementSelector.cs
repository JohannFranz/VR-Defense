using UnityEngine;

public class VRPlacementSelector : MonoBehaviour
{
    [SerializeField]
    private bool inVRMode;
    [SerializeField]
    private bool selected;

    private Placement placement;
    private Renderer placementRenderer;
    private VRPlacementSelector[] others;
    private Placement[] otherPlacements;

    void Start()
    {
        placement = GetComponent<Placement>();
        placementRenderer = GetComponent<Renderer>();
        
        GameObject[] placements = GameObject.FindGameObjectsWithTag(Constants.PlacementTag);
        others = new VRPlacementSelector[placements.Length - 1];
        otherPlacements = new Placement[others.Length];
        int i = 0;
        foreach (GameObject go in placements)
        {
            if (go == gameObject)
                continue;

            others[i] = go.GetComponent<VRPlacementSelector>();
            otherPlacements[i] = go.GetComponent<Placement>();
            i++;
        }

        selected = false;

        VR_Controller vrCon = GameObject.Find(Constants.VR_Tag).GetComponent<VR_Controller>();
        if (vrCon.IsVRMode())
        {
            inVRMode = true;
            return;
        }

        inVRMode = false;
        enabled = false;
    }

    public void OnPointerDown()
    {
        if (inVRMode == false)
            return;

        if (placement.IsAlreadyOccupied())
            return;

        SelectPlacement();
        DeselectAlreadySelectedPlacement();
    }

    private void SelectPlacement()
    {
        selected = true;
        placementRenderer.material.color = Constants.VR_PlacementSelectedColor;
    }

    private void DeselectAlreadySelectedPlacement()
    {
        for (int i = 0; i < others.Length; i++)
        {
            if (otherPlacements[i].IsAlreadyOccupied())
                continue;

            others[i].selected = false;
            others[i].placementRenderer.material.color = Constants.VR_PlacementUnselectedColor;
        }
    }
}
