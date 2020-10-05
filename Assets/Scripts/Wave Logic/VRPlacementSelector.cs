using System.Collections.Generic;
using UnityEngine;

public class VRPlacementSelector : MonoBehaviour
{
    [SerializeField]
    private bool inVRMode;
    [SerializeField]
    private bool selected;

    private Placement placement;
    private Renderer placementRenderer;
    private List<VRPlacementSelector> others;
    private List<Placement> otherPlacements;

    void Start()
    {
        placement = GetComponent<Placement>();
        placementRenderer = GetComponent<Renderer>();

        others = new List<VRPlacementSelector>();
        otherPlacements = new List<Placement>();

        FindPlacements();
        
        selected = false;
        GameObject canvasGO = transform.GetChild(0).gameObject;

        VR_Controller vrCon = GameObject.Find(Constants.VR_Tag).GetComponent<VR_Controller>();
        if (vrCon.IsVRMode())
        {
            inVRMode = true;
            Canvas can = canvasGO.GetComponent<Canvas>();
            can.worldCamera = GameObject.Find(Constants.VR_PointerTag).GetComponent<Camera>();
            return;
        }

        inVRMode = false;
        enabled = false;
        canvasGO.SetActive(false);
    }

    private void FindPlacements()
    {
        GameObject[] placementAreas = GameObject.FindGameObjectsWithTag(Constants.PlacementAreaTag);

        for (int i = 0; i < placementAreas.Length; i++)
        {
            for (int j = 0; j < placementAreas[i].transform.childCount; j++)
            {
                GameObject area = placementAreas[i].transform.GetChild(j).gameObject;

                if (area == gameObject)
                    continue;

                others.Add(area.GetComponent<VRPlacementSelector>());
                otherPlacements.Add(area.GetComponent<Placement>());
            }
        }
    }

    public void OnClick()
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
        for (int i = 0; i < others.Count; i++)
        {
            if (otherPlacements[i].IsAlreadyOccupied())
                continue;

            others[i].selected = false;
            others[i].placementRenderer.material.color = Constants.VR_PlacementUnselectedColor;
        }
    }

    public bool IsSelected()
    {
        return selected;
    }

    public void PlaceMerc(GameObject mercenary)
    {
        selected = false;
        placement.Occupy(mercenary);
    }
}
