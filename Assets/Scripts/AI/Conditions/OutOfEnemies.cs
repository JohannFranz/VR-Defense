using UnityEngine;

[CreateAssetMenu(menuName = "AI/Condition/OutOfEnemies")]
public class OutOfEnemies : TransitionCondition
{
    public override bool IsMet(GameObject agent)
    {
        AgentController controller = agent.GetComponent<AgentController>();
        return controller.sensor.GetComponent<AgentSensor>().GetEnemies().Count <= 0;
    }
}
