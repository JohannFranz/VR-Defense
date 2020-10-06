using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class EndlessGameController : MonoBehaviour
{
    public SceneLoader sceneLoader;
    public SpawnController[] spawnCons;
    public GoalController[] goals;
    public int numberLives;

    public GameObject gameOverView;
    public GameObject pauseView;

    private GameObject waveLevelText;
    private GameObject currentLivesText;
    private GameObject hand;
    private GameObject deck;
    private GameObject deckManager;
    private GameObject startButton;
    private GameObject startButtonText;
    private bool pausedGame;

    private GameObject[] placementAreas;
    private bool firstStart;

    void Awake()
    {
        GameObject camera = GameObject.FindGameObjectWithTag(Constants.CameraTag);
        Debug.Assert(camera != null);
        GameObject factory = GameObject.FindGameObjectWithTag(Constants.FactoryTag);
        Debug.Assert(factory != null);
        factory.GetComponent<Factory.AgentFactory>().cam = camera.transform;

        firstStart = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        waveLevelText = GameObject.FindGameObjectWithTag(Constants.WaveLevelTextTag);
        Debug.Assert(waveLevelText != null);
        currentLivesText = GameObject.FindGameObjectWithTag(Constants.CurrentLivesTextTag);
        Debug.Assert(currentLivesText != null);

        UpdateLifeText();

        hand = GameObject.FindGameObjectWithTag(Constants.HandTag);
        Debug.Assert(hand != null);
        deck = GameObject.FindGameObjectWithTag(Constants.DeckTag);
        Debug.Assert(deck != null);
        deckManager = GameObject.FindGameObjectWithTag(Constants.DeckManager);
        Debug.Assert(deckManager != null);
        startButton = GameObject.FindGameObjectWithTag(Constants.StartButtonTag);
        Debug.Assert(startButton != null);
        startButtonText = GameObject.FindGameObjectWithTag(Constants.StartButtonTextTag);
        Debug.Assert(startButtonText != null);

        startButton.GetComponent<Button>().onClick.AddListener(delegate { RunNextWave(); });

        pausedGame = false;
        gameOverView.SetActive(false);
        pauseView.SetActive(false);

        foreach (SpawnController spawnCon in spawnCons)
        {
            spawnCon.Init(WaveFinishedCallback);
        }

        for (int i = 0; i < goals.Length; i++)
        {
            goals[i].SetTriggerCallback(ReduceLives);
        }
        
        placementAreas = GameObject.FindGameObjectsWithTag(Constants.PlacementAreaTag);
        Debug.Assert(placementAreas != null);
    }

    void Update()
    {
        if (numberLives <= 0)
            GameOver();

        if (HasGameEnded())
            SuccessfulEnd();

        if (firstStart)
        {
            PrepareNextWave();
            firstStart = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsPaused())
            {
                ResumeGame();
            } else
            {
                PauseGame();
            }
        }
    }

    public void WaveFinishedCallback()
    {
        if (HasRoundEnded())
        {
            if (HasGameEnded())
            {
                SuccessfulEnd();
            }
            else
            {
                PrepareNextWave();
            }
        }
    }

    private bool HasGameEnded()
    {
        if (HasRoundEnded() == false)
            return false;

        foreach (SpawnController spawnCon in spawnCons)
        {
            if (spawnCon.HasWavesLeft())
                return false;
        }

        return true;
    }

    private bool HasRoundEnded()
    {
        foreach (SpawnController spawnCon in spawnCons)
        {
            if (spawnCon.IsWaveFinished() == false)
                return false;
        }

        return true;
    }

    private void UpdateWaveText()
    {
        int currentWave = spawnCons[0].GetCurrentWaveNumber();
        int maxWaves = spawnCons[0].GetMaximumWaves();
        waveLevelText.GetComponent<TextMeshProUGUI>().SetText(currentWave.ToString());
    }

    private void UpdateLifeText()
    {
        currentLivesText.GetComponent<TextMeshProUGUI>().SetText(numberLives.ToString());
    }

    public void PrepareNextWave()
    {
        UpdateWaveText();

        EnablePreparationView(true);
        foreach (GameObject pl in placementAreas)
        {
            pl.SetActive(true);
        }

        Card.Deck deck = deckManager.GetComponent<Card.Deck>();
        if (deck.IsHandFullWithCards())
        {
            EnableStartButton(true);
        }
        else
        {
            EnableStartButton(false);
        }

        int cardsInHand = hand.GetComponent<Card.HandPlacement>().GetAmountOfCards();
        deck.DrawCards(Constants.HandSize - cardsInHand, StopCardDrawingPhase);
    }

    public void RunNextWave()
    {
        EnablePreparationView(false);
        foreach (GameObject pl in placementAreas)
        {
            pl.SetActive(false);
        }
        for (int i = 0; i < spawnCons.Length; i++)
        {
            spawnCons[i].StartNextWave();
        }
    }

    public void SuccessfulEnd()
    {
        ProcessEnd(true);
    }

    public void GameOver()
    {
        ProcessEnd(false);
    }

    public void ProcessEnd(bool successfulEnd)
    {
        gameOverView.SetActive(true);
        GameObject text = gameOverView.transform.Find("GameOverText").gameObject;
        if (successfulEnd)
        {
            text.GetComponent<TextMeshProUGUI>().SetText("Congratulations!!!");
        }
        else
        {
            text.GetComponent<TextMeshProUGUI>().SetText("Game Over!!!");
        }
    }

    public void RestartGame()
    {
        sceneLoader.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1.0f;
        gameOverView.SetActive(false);
        pauseView.SetActive(false);
        EnablePreparationView(false);
        foreach (GameObject pl in placementAreas)
        {
            pl.SetActive(false);
        }
        sceneLoader.LoadMainMenu();
    }

    //Based on Brackeys-Video:"Pause Menu in Unity"
    public void PauseGame()
    {
        pausedGame = true;
        Time.timeScale = 0.0f;
        pauseView.SetActive(true);
    }

    public void ResumeGame()
    {
        pausedGame = false;
        Time.timeScale = 1.0f;
        pauseView.SetActive(false);
    }

    public bool IsPaused()
    {
        return pausedGame;
    }

    public void StopCardDrawingPhase()
    {
        EnableStartButton(true);
    }

    private void EnableStartButton(bool enable)
    {
        startButton.GetComponent<UnityEngine.UI.Image>().raycastTarget = enable;
        
        if (enable)
        {
            startButtonText.GetComponent<TextMeshProUGUI>().SetText("Start <br> Round");
        }
        else
        {
            startButtonText.GetComponent<TextMeshProUGUI>().SetText("Wait for <br> it...");
        }
    }

    private void EnablePreparationView(bool enable)
    {
        hand.SetActive(enable);
        startButton.SetActive(enable);
        startButtonText.SetActive(enable);
        deck.SetActive(enable);
    }

    public void ReduceLives()
    {
        numberLives -= 1;
        UpdateLifeText();
    }
}
