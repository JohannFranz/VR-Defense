using System.Collections;
using Valve.VR;
using UnityEngine;

//Code based on  https://www.youtube.com/watch?v=-T09oRMDuG8
public class Teleporter : MonoBehaviour
{
    public GameObject pointer;
    public SteamVR_Action_Boolean teleportAction;

    private SteamVR_Behaviour_Pose pose;
    private bool hasPosition;
    private bool isTeleporting;
    private float fadeTime;

    [SerializeField]
    private bool inVRMode;

    // Start is called before the first frame update
    void Start()
    {
        VR_Controller vrCon = GameObject.Find(Constants.VR_Tag).GetComponent<VR_Controller>();
        if (vrCon.IsVRMode())
            inVRMode = true;
        else
            inVRMode = false;

        pose = GetComponent<SteamVR_Behaviour_Pose>();
        hasPosition = false;
        isTeleporting = false;
        fadeTime = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (inVRMode == false)
            return;

        hasPosition = UpdatePointer();
        pointer.SetActive(hasPosition);

        if (teleportAction.GetLastStateUp(pose.inputSource))
            TryTeleport();
    }

    public void TryTeleport()
    {
        if (hasPosition == false || isTeleporting)
            return;

        Transform cameraRig = SteamVR_Render.Top().origin;
        Vector3 headPosition = SteamVR_Render.Top().head.position;

        Vector3 groundPosition = new Vector3(headPosition.x, cameraRig.position.y, headPosition.z);
        Vector3 translateVector = pointer.transform.position - groundPosition;

        StartCoroutine(MoveRig(cameraRig, translateVector));
    }

    private IEnumerator MoveRig(Transform cameraRig, Vector3 translation)
    {
        isTeleporting = true;
        SteamVR_Fade.Start(Color.black, fadeTime, true);

        yield return new WaitForSeconds(fadeTime);
        cameraRig.position += translation;

        SteamVR_Fade.Start(Color.clear, fadeTime, true);

        isTeleporting = false;
    }

    private bool UpdatePointer()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, Constants.GroundLayer))
        {
            pointer.transform.position = hit.point;
            return true;
        }
        return false;
    }
}
