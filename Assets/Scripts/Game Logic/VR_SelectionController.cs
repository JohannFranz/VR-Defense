using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VR_SelectionController : MonoBehaviour
{
    private VR_Controller vrCon;

    // Start is called before the first frame update
    void Start()
    {
        vrCon = GetComponent<VR_Controller>();

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = vrCon.GetPointerRay();
        if (Physics.Raycast(ray, out hit, 100.0f) == false)
            return;

        GameObject target = hit.rigidbody.gameObject;

        if (target.tag != Constants.PlacementTag)
            return;

        if (target.GetComponent<Placement>().IsAlreadyOccupied())
            return;


    }
}
