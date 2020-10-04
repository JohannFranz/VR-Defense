using UnityEngine;

public class SceneLoadHelper : MonoBehaviour
{
    void Start()
    {
        GameObject.Find(Constants.VR_Tag).GetComponent<VR_Controller>().ProcessSceneLoad();
    }

}
