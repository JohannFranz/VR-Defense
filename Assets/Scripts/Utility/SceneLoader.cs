using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public Animator sceneTransition;

    public void LoadScene(string name)
    {
        StartCoroutine(LoadLevel(name));
    }

    public void LoadMainMenu()
    {
        StartCoroutine(LoadLevel(Constants.Main_Menu));
    }

    public void LoadWorld(int index)
    {
        switch (index)
        {
            case 0:
                StartCoroutine(LoadLevel(Constants.World_Western));
                break;
            case 1:
                StartCoroutine(LoadLevel(Constants.World_Vineyard));
                break;
            case 2:
                StartCoroutine(LoadLevel(Constants.World_Medieval));
                break;
            case 3:
                StartCoroutine(LoadLevel(Constants.World_Ice));
                break;
            case 4:
                StartCoroutine(LoadLevel(Constants.World_Skull_Island));
                break;
        }
    }

    //Based on Brackeys-Video:"How to make AWESOME Scene Transitions in Unity"
    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        sceneTransition.SetTrigger(Constants.TriggerStart);

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(levelIndex);
    }

    IEnumerator LoadLevel(string levelName)
    {
        sceneTransition.SetTrigger(Constants.TriggerStart);

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(levelName);
    }
}
