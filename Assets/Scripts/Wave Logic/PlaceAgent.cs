using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceAgent : MonoBehaviour
{
    public float translationVelocity;

    private Camera cam;
    private RaycastHit hitAgent;
    private VR_Controller vrcon;

    //stores current mouse position
    private Vector3 currentPosition;
    private Vector3 zeroXRotation;
    private Transform selectedAgent;
    private Vector3 initialPosition;

    // Start is called before the first frame update
    void Start()
    {
        hitAgent = new RaycastHit();
        initialPosition = Constants.NullVector3;
        zeroXRotation = new Vector3(0, 0, 0);

        GameObject vrconGO = GameObject.Find(Constants.VR_Tag);

        if (vrconGO == null)
            cam = gameObject.GetComponent<Camera>();
        else
        {
            vrcon = vrconGO.GetComponent<VR_Controller>();
            if (vrcon.IsVRMode() == false)
                cam = gameObject.GetComponent<Camera>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool mousePressed = Input.GetMouseButton(0);
        currentPosition = Input.mousePosition;

        if (IsAgentDeselected(mousePressed))
        {
            Place();
        }

        if (IsAgentAlreadySelected(mousePressed))
        {
            MoveAgent();
            return;
        }

        if (mousePressed == false || IsAgentHit() == false)
        {
            initialPosition = Constants.NullVector3;
            return;
        }

        PrepareAgentForMovement();
    }

    private bool IsAgentDeselected(bool mousePressed)
    {
        return mousePressed == false && initialPosition != Constants.NullVector3;
    }

    private bool IsAgentAlreadySelected(bool mousePressed)
    {
        return mousePressed && initialPosition != Constants.NullVector3;
    }

    private bool IsAgentHit()
    {
        Ray ray;
        if (vrcon.IsVRMode())
        {
            ray = vrcon.GetPointerRay();
        }
        else
        {
            ray = cam.ScreenPointToRay(currentPosition);
        }
        bool wasHit = Physics.Raycast(ray.origin, ray.direction, out hitAgent, 100, Physics.DefaultRaycastLayers);
        if (wasHit == false)
            return false;

        if (hitAgent.rigidbody == null)
            return false;

        return true;
    }

    private void PrepareAgentForMovement()
    {
        hitAgent.collider.enabled = false;
        selectedAgent = hitAgent.transform;
        initialPosition = selectedAgent.position;
    }

    private void MoveAgent()
    {
        RaycastHit hit;
        Ray ray;
        if (vrcon.IsVRMode())
        {
            ray = vrcon.GetPointerRay();
        }
        else
        {
            ray = cam.ScreenPointToRay(currentPosition);
        }
        bool wasHit = Physics.Raycast(ray.origin, ray.direction, out hit, 1000/*, Physics.DefaultRaycastLayers*/);


        if (wasHit)
        {
            Vector3 newPosition = hit.point;
            selectedAgent.position = newPosition;

            Debug.DrawRay(ray.origin, ray.direction * hit.distance);
        }
    }

    private void Place()
    {
        RaycastHit hit;
        Ray ray;
        if (vrcon.IsVRMode())
        {
            ray = vrcon.GetPointerRay();
        }
        else
        {
            ray = cam.ScreenPointToRay(currentPosition);
        }
        bool wasHit = Physics.Raycast(ray.origin, ray.direction, out hit, 100/*, Physics.DefaultRaycastLayers*/);
        //Debug.DrawRay(ray.origin, ray.direction * (hit.point - ray.origin).magnitude);

        if (wasHit && hit.transform.CompareTag(Constants.PlacementTag))
        {
            selectedAgent.position = hit.transform.position;
            selectedAgent.GetComponent<AgentController>().goal = hit.collider.gameObject;
        } else
        {
            ResetAgent();
        }

        hitAgent.collider.enabled = true;
    }

    private void ResetAgent()
    {
        selectedAgent.position = initialPosition;
        initialPosition = Constants.NullVector3;
    }
}
