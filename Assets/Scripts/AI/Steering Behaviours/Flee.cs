using UnityEngine;

//Code based on AI for Games 3rd Edition
public class Flee : SteeringBehaviour
{
    public override void Init(GameObject self)
    {
        base.Init(self);
    }

    //Code from AI for Games 3rd Edition, p. 60
    public override SteeringOutput getSteering(Kinematic selfKinematic, GameObject target = null, Kinematic targetKinematic = null)
    {
        if (target == null)
            return null;
        result.linear = selfKinematic.position - target.transform.position;
        //project onto plane
        result.linear.y = 0;

        result.linear.Normalize();
        result.linear *= selfKinematic.maxAcceleration;

        result.angular = 0;
        return result;
    }
}
