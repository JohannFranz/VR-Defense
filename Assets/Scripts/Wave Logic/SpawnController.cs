using UnityEngine;
using System;

public class SpawnController : MonoBehaviour
{
    public Transform goal;
    public Wave[] waves;

    private int currentWave;
    private bool isWaveFinished;
    private bool hasWavesLeft;

    private Action waveFinishedCallback;
    private bool callbackCalled;

    // Start is called before the first frame update
    void Start()
    {
        currentWave = -1;
        isWaveFinished = false;
        hasWavesLeft = true;
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

        if (currentWave + 1 == waves.Length)
            hasWavesLeft = false;
    }

    public void Init(Action callback)
    {
        waveFinishedCallback = callback;
    }

    public bool HasWavesLeft()
    {
        return hasWavesLeft;
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
        currentWave += 1;
        waves[currentWave].Activate(gameObject, goal.gameObject);
        isWaveFinished = false;
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
