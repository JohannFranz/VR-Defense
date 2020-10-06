using UnityEngine;
using System;

public class SpawnController : MonoBehaviour
{
    public Transform goal;
    public Wave[] waves;

    [SerializeField]
    private int currentWave;
    [SerializeField]
    private bool isWaveFinished;

    private Action waveFinishedCallback;
    private bool callbackCalled;

    // Start is called before the first frame update
    void Start()
    {
        currentWave = -1;
        isWaveFinished = false;
        callbackCalled = false;
    }

    void Update()
    {
        if (currentWave < 0)
            return;

        if (waves[currentWave].IsFinished())
        {
            isWaveFinished = true;
            if (callbackCalled == false)
            {
                waveFinishedCallback();
                callbackCalled = true;
            }
        }
    }

    public void Init(Action callback)
    {
        waveFinishedCallback = callback;
    }

    public bool HasWavesLeft()
    {
        return currentWave + 1 != waves.Length;
    }

    public bool IsWaveFinished()
    {
        return isWaveFinished;
    }

    private bool IsWaveSleeping()
    {
        if (currentWave >= waves.Length)
            return false;

        return waves[currentWave].IsSleeping();
    }

    public void StartNextWave()
    {
        isWaveFinished = false;
        currentWave += 1;
        waves[currentWave].Activate(gameObject, goal.gameObject);
        callbackCalled = false;
    }

    public int GetCurrentWaveNumber()
    {
        return currentWave + 2;
    }

    public int GetMaximumWaves()
    {
        return waves.Length;
    }
}
