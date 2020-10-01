using UnityEngine;

[CreateAssetMenu(menuName = "AI/Condition/EnemyInViewRange")]
public class EnemyInViewRange : TransitionCondition
{
    public override bool IsMet(GameObject agent)
    {
        AgentController controller = agent.GetComponent<AgentController>();
        return controller.sensor.GetEnemies().Count > 0;
    }
}
