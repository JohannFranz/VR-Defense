using UnityEngine;

public class Separation : SteeringBehaviour
{
    public GameObject[] targets;
    public float threshold;
    public float decayCoefficient;

    float thresholdSquared;

    public override void Init(GameObject self)
    {
        base.Init(self);

        SetThreshold(threshold);

        //####################  Todo: Ersetzen mit Teammitgliedern   ########################
        GameObject[] minions = GameObject.FindGameObjectsWithTag(Constants.MinionTag);
        targets = new GameObject[Mathf.Max(minions.Length - 1, 0)];
        int parentInstanceID = self.GetInstanceID();
        int j = 0;
        for (int i = 0; i < minions.Length; i++)
        {
            if (minions[i].GetInstanceID() == parentInstanceID)
                continue;
            targets[j] = minions[i];
            j += 1;
        }
    }

    //Code based on AI for Games 3rd Edition, p. 83
    public override SteeringOutput getSteering(Kinematic selfKinematic, GameObject target = null, Kinematic targetKinematic = null)
    {
        if (targets.Length == 0)
            return null;

        result.angular = 0;
        result.linear = Vector3.zero;

        foreach (GameObject t in targets)
        {
            Vector3 direction = selfKinematic.position - t.transform.position;
            //project onto plane
            direction.y = 0;
            float distanceSquared = direction.sqrMagnitude;

            if (distanceSquared < thresholdSquared)
            {
                float strength = Mathf.Min(decayCoefficient / distanceSquared, selfKinematic.maxAcceleration);
                direction.Normalize();
                result.linear += strength * direction * weight;
            }
        }
        return result;
    }

    public void SetThreshold(float threshold)
    {
        //Add character radius to threshold
        float characterRadius = self.GetComponent<CapsuleCollider>().radius;
        threshold += 2 * characterRadius;

        thresholdSquared = threshold * threshold;
    }
}
