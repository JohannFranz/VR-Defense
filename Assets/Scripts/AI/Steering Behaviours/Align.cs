using UnityEngine;

public class Align : SteeringBehaviour
{
    public float targetRadius;
    public float slowRadius;
    public float timeToTarget;

    public override void Init(GameObject self)
    {
        base.Init(self);
    }

    //Code from Unity 5.x Game AI Cookbook p. 12
    public float MapToRange(float rotation)
    {
        rotation %= 360.0f;
        if (Mathf.Abs(rotation) > 180.0f)
        {
            if (rotation < 0.0f)
                rotation += 360.0f;
            else
                rotation -= 360.0f;
        }
        return rotation;
    }

    //Code based on AI for Games 3rd Edition, p. 65
    public override SteeringOutput getSteering(Kinematic selfKinematic, GameObject target = null, Kinematic targetKinematic = null)
    {
        if (targetKinematic == null)
            return null;

        float rotation = targetKinematic.orientation - selfKinematic.orientation;
        rotation = MapToRange(rotation);
        float rotationSize = Mathf.Abs(rotation);

        if (rotationSize < targetRadius)
            return null;

        float targetRotation = 0;

        if (rotationSize > slowRadius)
            targetRotation = selfKinematic.maxRotation;
        else
            targetRotation = selfKinematic.maxRotation * rotationSize / slowRadius;

        targetRotation *= rotation / rotationSize;
        result.angular = targetRotation - selfKinematic.rotation;
        result.angular /= timeToTarget;

        float angularAcceleration = Mathf.Abs(result.angular);

        if (angularAcceleration > selfKinematic.maxAngularAcceleration)
        {
            result.angular /= angularAcceleration;
            result.angular *= selfKinematic.maxAngularAcceleration;
        }
        result.linear = Vector3.zero;

        return result;
    }
}
