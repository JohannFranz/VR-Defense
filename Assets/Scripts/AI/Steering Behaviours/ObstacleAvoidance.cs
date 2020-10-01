using UnityEngine;

public class ObstacleAvoidance : Seek
{
    public float avoidDistance;
    public float lookAhead;

    GameObject seekTarget;

    public override void Init(GameObject self)
    {
        base.Init(self);
        seekTarget = new GameObject();
    }
    
    //Code based on AI for Games 3rd Edition, p. 91
    public override SteeringOutput getSteering(Kinematic selfKinematic, GameObject target = null, Kinematic targetKinematic = null)
    {
        Vector3 rayDir = selfKinematic.velocity;
        rayDir.Normalize();
        rayDir *= lookAhead;

        RaycastHit hit;
        if (Physics.Raycast(selfKinematic.position, rayDir, out hit, lookAhead) == false)
            return null;
        if (hit.rigidbody == null)
            return null;
        if (hit.rigidbody.gameObject.tag == Constants.MinionTag)
            return null;
        seekTarget.transform.position = hit.point + hit.normal * avoidDistance;

        return base.getSteering(selfKinematic, seekTarget, null);
    }
}
