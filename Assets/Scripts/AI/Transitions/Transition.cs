using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Transition")]
public class Transition : ScriptableObject
{
    public State previousState;
    public State nextState;
    public List<TransitionCondition> conditions;

    public void Init()
    {
        previousState.InitID();
        nextState.InitID();
    }

    public State Evaluate(GameObject agent)
    {
        foreach(TransitionCondition condition in conditions)
        {
            if (condition.IsMet(agent) == false)
                return null;
        }

        return nextState;
    }
}
