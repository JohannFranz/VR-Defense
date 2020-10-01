using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Condition/EnemyInShootRange")]
public class EnemyInShootRange : TransitionCondition
{
    public override bool IsMet(GameObject agent)
    {
        AgentController controller = agent.GetComponent<AgentController>();
        List<GameObject> enemies = controller.sensor.GetEnemies();
        foreach (GameObject enemy in enemies)
        {
            float distance = (enemy.transform.position - agent.transform.position).magnitude;
            if (distance <= controller.attackRange)
                return true;
        }
        return false;
    }
}
