using UnityEngine;

public class LookingWhereYoureGoing : Align
{

    Kinematic alignTarget; //This target will be delegated to the align-behaviour

    public override void Init(GameObject self)
    {
        base.Init(self);
        alignTarget = new Kinematic();
    }

    //Code based on AI for Games 3rd Edition, p. 73
    public override SteeringOutput getSteering(Kinematic selfKinematic, GameObject target = null, Kinematic targetKinematic = null)
    {
        if (selfKinematic.velocity.sqrMagnitude == 0)
            return null;

        alignTarget.orientation = Mathf.Atan2(selfKinematic.velocity.x, selfKinematic.velocity.z) * Mathf.Rad2Deg;
        return base.getSteering(selfKinematic, target, alignTarget);
    }
}
