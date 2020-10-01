using UnityEngine;

[CreateAssetMenu(menuName = "AI/Condition/HasFormationSlot")]
public class HasFormationSlot : TransitionCondition
{
    public override bool IsMet(GameObject agent)
    {
        AgentController controller = agent.GetComponent<AgentController>();
        if (controller.teamLeader == null)
            return false;

        GameObject childAI = TD_Utility.Utility.GetChildByTag(controller.teamLeader, Constants.AITag);
        if (childAI.GetComponent<TeamController>().IsTeamLeader())
            return true;

        return childAI.GetComponent<TeamController>().IsPartOfFormation(agent);
    }
}
