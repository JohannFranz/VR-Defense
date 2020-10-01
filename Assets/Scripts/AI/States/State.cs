using UnityEngine;

public abstract class State : ScriptableObject
{
    private static int Static_ID = 0;
    private int id;
    protected bool wasInitialized = false;

    public int GetID()
    {
        return id;
    }

    public void InitID()
    {
        id = GetStateID();
    }

    private static int GetStateID()
    {
        Static_ID += 1;
        return Static_ID;
    }

    public abstract void EnterState(GameObject agent);
    public abstract void ExitState(GameObject agent);
    public abstract void UpdateState(GameObject agent);
}