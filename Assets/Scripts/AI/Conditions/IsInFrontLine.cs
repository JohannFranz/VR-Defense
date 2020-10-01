using UnityEngine;

[CreateAssetMenu(menuName = "AI/Condition/IsInFrontLine")]
public class IsInFrontLine : TransitionCondition
{
    public override bool IsMet(GameObject agent)
    {
        AgentController controller = agent.GetComponent<AgentController>();
        if (controller.teamLeader == null)
            return false;

        GameObject childAI = TD_Utility.Utility.GetChildByTag(controller.teamLeader, Constants.AITag);
        TeamController teamCon = childAI.GetComponent<TeamController>();

        return teamCon.IsInFront(agent);
    }
}