using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public SceneLoader sceneLoader;
    public GameObject mainView;
    public GameObject worldView;

    public void ShowWorldSelection()
    {
        mainView.SetActive(false);
        worldView.SetActive(true);
    }

    public void ShowMainView()
    {
        mainView.SetActive(true);
        worldView.SetActive(false);
    }

    public void StartWorld(int index)
    {
        sceneLoader.LoadWorld(index);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
