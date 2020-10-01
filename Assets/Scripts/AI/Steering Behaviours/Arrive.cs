using UnityEngine;

public class Arrive : SteeringBehaviour
{
    public float targetRadiusSquared;
    public float slowRadiusSquared;
    public float timeToTarget;

    public override void Init(GameObject self)
    {
        base.Init(self);
    }

    //Code based on AI for Games 3rd Edition, p. 62
    public override SteeringOutput getSteering(Kinematic selfKinematic, GameObject target = null, Kinematic targetKinematic = null)
    {
        if (target == null)
            return null;

        float targetSpeed = 0;
        Vector3 targetVelocity;

        Vector3 direction = target.transform.position - selfKinematic.position;
        //project onto plane
        direction.y = 0;
        float distanceSquared = direction.sqrMagnitude;

        if (distanceSquared < targetRadiusSquared)
            return null;

        if (distanceSquared > slowRadiusSquared)
            targetSpeed = selfKinematic.maxVelocity;
        else
            targetSpeed = selfKinematic.maxVelocity * Mathf.Sqrt(distanceSquared) / Mathf.Sqrt(slowRadiusSquared);

        targetVelocity = direction;
        targetVelocity.Normalize();
        targetVelocity *= targetSpeed;

        result.linear = targetVelocity - selfKinematic.velocity;
        result.linear /= timeToTarget;

        if (result.linear.sqrMagnitude > selfKinematic.maxAccelerationSquared)
        {
            result.linear.Normalize();
            result.linear *= selfKinematic.maxAcceleration;
        }
        result.linear *= weight;
        result.angular = 0;
        return result;
    }
}
