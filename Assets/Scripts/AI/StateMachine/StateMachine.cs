using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public State startState;
    public Transition[] transitions;

    protected GameObject agent; //the current agent running the state machine
    [SerializeField]
    protected State currentState;
    protected State nextState;
    protected List<Transition> currentTransitions;

    void Awake()
    {
        foreach (Transition trans in transitions)
        {
            trans.Init();
        }

        currentState = startState;
        nextState = null;
        currentTransitions = new List<Transition>();
        FilterCurrentTransitions();
    }

    void Start()
    {
        currentState.EnterState(agent);
    }

    void Update()
    {
        EvaluateTransitions();
        if (nextState == null)
        {
            currentState.UpdateState(agent);
            return;
        }

        TransitionToNextState();
        currentState.UpdateState(agent);
    }

    private void TransitionToNextState()
    {
        currentState.ExitState(agent);
        nextState.EnterState(agent);
        currentState = nextState;
        nextState = null;
        FilterCurrentTransitions();
    }

    public void SetAgent(GameObject agent)
    {
        this.agent = agent;
    }

    private void EvaluateTransitions()
    {
        foreach (Transition trans in currentTransitions)
        {
            nextState = trans.Evaluate(agent);
            if (nextState != null) break;
        }
    }

    private void FilterCurrentTransitions()
    {
        currentTransitions.Clear();
        foreach (Transition trans in transitions)
        {
            if (currentState.GetID() != trans.previousState.GetID()) continue;

            currentTransitions.Add(trans);
        }
    }
}
