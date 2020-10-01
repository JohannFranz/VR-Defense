using UnityEngine;

public abstract class TransitionCondition : ScriptableObject
{
    public abstract bool IsMet(GameObject agent);
}

