using UnityEngine;

[CreateAssetMenu(menuName = "AI/Condition/HasFormationSlotReached")]
public class HasFormationSlotReached : TransitionCondition
{
    public override bool IsMet(GameObject agent)
    {
        GameObject childAI = TD_Utility.Utility.GetChildByTag(agent, Constants.AITag);
        return childAI.GetComponent<KinematicController>().HasReachedTarget();
    }
}
