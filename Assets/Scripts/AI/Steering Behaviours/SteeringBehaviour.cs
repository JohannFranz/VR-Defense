using UnityEngine;

//Based on AI for Games 3rd Edition
public class SteeringOutput
{
    public Vector3 linear;//linear acceleration
    public float angular; //angular acceleration
}

//Based on AI for Games 3rd Edition
public class Kinematic
{
    public Vector3 position;
    public Vector3 velocity;
    public float maxVelocity;
    public float maxVelocitySquared;
    public float maxAcceleration;
    public float maxAccelerationSquared;

    public float orientation;
    public float rotation;
    public float maxRotation;
    public float maxAngularAcceleration;
}

public class SteeringBehaviour : MonoBehaviour
{
    public SteeringOutput result;
    public float weight = 1;

    protected GameObject self;

    public virtual void Init(GameObject self)
    {
        this.self = self;
        result = new SteeringOutput();
    }

    public virtual SteeringOutput getSteering(Kinematic selfKinematic, GameObject target = null, Kinematic targetKinematic = null) { return result; }
}

