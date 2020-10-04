using UnityEngine;
using UnityEngine.XR;
using UnityEngine.SceneManagement;

public class VR_Controller : MonoBehaviour
{
    static VR_Controller vrInstance;

    private bool vrModeActive;
    private GameObject vrPointer;

    void Awake()
    {
        if (vrInstance != null)
        {
            Destroy(gameObject);
            return;
        }

        vrInstance = this;
        DontDestroyOnLoad(gameObject);

        if (XRDevice.isPresent)
            vrModeActive = true;
        else
            vrModeActive = false;

        vrPointer = GameObject.Find(Constants.VR_PointerTag);
    }

    private void SetMainMenuVRMode()
    {
        Camera.main.gameObject.SetActive(false);

        GameObject canvas = GameObject.Find(Constants.CanvasTag);
        Canvas can = canvas.GetComponent<Canvas>();
        can.renderMode = RenderMode.WorldSpace;
        can.worldCamera = GameObject.Find(Constants.VR_PointerTag).GetComponent<Camera>();

        RectTransform rec = canvas.GetComponent<RectTransform>();
        rec.position = Constants.VR_CanvasMainMenuPosition;
        rec.transform.localScale = Constants.VR_CanvasScale;
    }

    private void SetMainMenuStandardMode()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        GameObject.Find("Plane").SetActive(false);
    }

    private void SetWorldVRMode()
    {
        Camera.main.gameObject.SetActive(false);

        GameObject canvas = GameObject.Find(Constants.CanvasTag);
        Canvas can = canvas.GetComponent<Canvas>();
        can.renderMode = RenderMode.WorldSpace;
        can.worldCamera = vrPointer.GetComponent<Camera>();

        RectTransform rec = canvas.GetComponent<RectTransform>();
        rec.position = Constants.VR_CanvasGamePosition;
        rec.Rotate(Constants.VR_CanvasGameRotation);
        rec.transform.localScale = Constants.VR_CanvasScale;
    }

    private void SetWorldStandardMode()
    {

    }

    public void ProcessSceneLoad()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == Constants.Main_Menu)
        {
            if (vrModeActive)
                SetMainMenuVRMode();
            else
                SetMainMenuStandardMode();
        }
        else
        {
            if (vrModeActive)
                SetWorldVRMode();
            else
                SetWorldStandardMode();
        }
    }

    public Ray GetPointerRay()
    {
        return vrPointer.GetComponent<Pointer>().GetPointerRay();
    }

    public bool IsVRMode()
    {
        return vrModeActive;
    }
}
