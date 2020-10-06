using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//Code based on https://www.youtube.com/watch?v=3mRI1hu9Y3w&list=PLmc6GPFDyfw85CcfwbB7ptNVJn5BSBaXz
public class Pointer : MonoBehaviour
{
    [SerializeField] private GameObject dot = null;

    public Camera Camera { get; private set; } = null;

    private LineRenderer lineRenderer = null;
    private VRModuleInput inputModule = null;

    private Ray pointerRay;

    [SerializeField]
    private bool inVRMode;

    private void Awake()
    {
        VR_Controller vrCon = GameObject.Find(Constants.VR_Tag).GetComponent<VR_Controller>();
        if (vrCon.IsVRMode())
            inVRMode = true;
        else
            inVRMode = false;

        Camera = GetComponent<Camera>();
        Camera.enabled = false;

        lineRenderer = GetComponent<LineRenderer>();

        pointerRay = new Ray(transform.position, transform.forward);
    }

    private void Start()
    {
        // current.currentInputModule does not work
        inputModule = EventSystem.current.gameObject.GetComponent<VRModuleInput>();
    }

    private void Update()
    {
        if (inVRMode == false)
            return;

        UpdateLine();
    }

    private void UpdateLine()
    {
        // Use default or distance
        PointerEventData data = inputModule.Data;
        RaycastHit hit;
        pointerRay.origin = transform.position;
        pointerRay.direction = transform.forward;
        Physics.Raycast(pointerRay, out hit, Constants.VR_PointerLength, Constants.UILayer);

        // If nothing is hit, set do default length
        float colliderDistance = hit.distance == 0 ? Constants.VR_PointerLength : hit.distance;
        float canvasDistance = data.pointerCurrentRaycast.distance == 0 ? Constants.VR_PointerLength : data.pointerCurrentRaycast.distance;

        // Get the closest one
        float targetLength = Mathf.Min(colliderDistance, canvasDistance);

        // Default
        Vector3 endPosition = transform.position + (transform.forward * targetLength);

        // Set position of the dot
        dot.transform.position = endPosition;

        // Set linerenderer
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, endPosition);
    }

    public Ray GetPointerRay()
    {
        return pointerRay;
    }
}

//public GameObject dot;
//public VRModuleInput inputModule;

//private LineRenderer lineRenderer;
//private Ray pointerRay;

//void Awake()
//{
//    lineRenderer = GetComponent<LineRenderer>();
//    pointerRay = new Ray(transform.position, transform.forward);
//}

//// Update is called once per frame
//void Update()
//{
//    UpdateLine();
//}

//private void UpdateLine()
//{
//    PointerEventData data = inputModule.GetData();
//    float targetLength = data.pointerCurrentRaycast.distance == 0 ? Constants.VR_PointerLength : data.pointerCurrentRaycast.distance;
//    Vector3 endPosition = transform.position + (transform.forward * targetLength);

//    RaycastHit hit;
//    pointerRay.origin = transform.position;
//    pointerRay.direction = transform.forward;
//    if (Physics.Raycast(pointerRay, out hit, Constants.VR_PointerLength))
//    {
//        endPosition = hit.point;
//    }

//    dot.transform.position = endPosition;

//    lineRenderer.SetPosition(0, transform.position);
//    lineRenderer.SetPosition(1, endPosition);
//}

//public Ray GetPointerRay()
//{
//    return pointerRay;
//}