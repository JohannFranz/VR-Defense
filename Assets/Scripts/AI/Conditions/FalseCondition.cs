using UnityEngine;

//Useful when a state is suspposed to loop forever
[CreateAssetMenu(menuName = "AI/Condition/FalseCondition")]
public class FalseCondition : TransitionCondition
{
    public override bool IsMet(GameObject agent)
    {
        return false;
    }
}
