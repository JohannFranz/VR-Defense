using UnityEngine;
using Valve.VR;
using UnityEngine.XR;

public class VR_Controller : MonoBehaviour
{
    //public XRLoader WMRLoader;
    //public XRLoader MockLoader;

    //void Awake()
    //{
    //    XRGeneralSettings.Instance.Manager.loaders.Clear();

    //    //Initialize WMR.
    //    XRGeneralSettings.Instance.Manager.loaders.Add(WMRLoader);
    //    XRGeneralSettings.Instance.Manager.InitializeLoaderSync();
    //    XRGeneralSettings.Instance.Manager.StartSubsystems();

    //    //Check if initialization was successfull.
    //    var xrDisplaySubsystems = new List<XRDisplaySubsystem>();
    //    SubsystemManager.GetInstances(xrDisplaySubsystems);
    //    bool success = xrDisplaySubsystems[0].running;

    //    if (!success)
    //    {
    //        Debug.Log("success");
    //        //Initialization was not successfull, load mock instead.
    //        print("loading mock");

    //        //Deinitialize WMR
    //        XRGeneralSettings.Instance.Manager.loaders.Clear();
    //        XRGeneralSettings.Instance.Manager.StopSubsystems();
    //        XRGeneralSettings.Instance.Manager.DeinitializeLoader();

    //        //Initialize mock.
    //        XRGeneralSettings.Instance.Manager.loaders.Add(MockLoader);
    //        XRGeneralSettings.Instance.Manager.InitializeLoaderSync();
    //        XRGeneralSettings.Instance.Manager.StartSubsystems();
    //    } else
    //    {
    //        Debug.Log("fail");
    //    }
    //}

    void Awake()
    {
        
        Debug.Log("Device is present in Awake: " + XRDevice.isPresent);
    }

    void Start()
    {
        Debug.Log("Device is present in Start: " + XRDevice.isPresent);
    }


    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < SteamVR.connected.Length; i++)
        {
            if (SteamVR.connected[i])
                Debug.Log("Connected in " + i);
        }

        Debug.Log("Device is present in Update: " + XRDevice.isPresent);
    }

    private void SwitchToVRMode()
    {
        Camera.main.gameObject.SetActive(false);

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }
}
