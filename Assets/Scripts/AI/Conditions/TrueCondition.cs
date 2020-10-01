using UnityEngine;

//Useful when a state is suspposed to only run once and change immediately
[CreateAssetMenu(menuName = "AI/Condition/TrueCondition")]
public class TrueCondition : TransitionCondition
{
    public override bool IsMet(GameObject agent)
    {
        return true;
    }
}
