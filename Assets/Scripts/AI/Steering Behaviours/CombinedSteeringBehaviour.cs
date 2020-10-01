using UnityEngine;

public class CombinedSteeringBehaviour : MonoBehaviour
{
    SteeringBehaviour[] behaviours;
    SteeringOutput output;

    // Start is called before the first frame update
    void Start()
    {
        behaviours = new SteeringBehaviour[4];
        behaviours[0] = GetComponent<LookingWhereYoureGoing>();
        behaviours[1] = GetComponent<Arrive>();
        behaviours[2] = GetComponent<Separation>();
        behaviours[3] = GetComponent<ObstacleAvoidance>();

        GameObject parent = transform.parent.gameObject;
        for (int i = 0; i < behaviours.Length; i++)
        {
            behaviours[i].Init(parent.transform.parent.gameObject);
        }

        output = new SteeringOutput();
    }

    public SteeringOutput getSteering(Kinematic selfKinematic, GameObject target = null, Kinematic targetKinematic = null)
    {
        output.linear = Vector3.zero;
        output.angular = 0;

        foreach (SteeringBehaviour behaviour in behaviours)
        {
            SteeringOutput steering = behaviour.getSteering(selfKinematic, target, targetKinematic);
            if (steering == null)
                continue;
            output.linear += steering.linear;
            output.angular += steering.angular;
        }

        return output;
    }
}
