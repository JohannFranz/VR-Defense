using UnityEngine;

public class Face : Align
{
    Kinematic alignTarget; //This target will be delegated to the align-behaviour

    public override void Init(GameObject self)
    {
        base.Init(self);
        alignTarget = new Kinematic();
    }

    void Start()
    {
        if (alignTarget == null)
            Init(transform.parent.gameObject.transform.parent.gameObject);
    }

    //Code based on AI for Games 3rd Edition, p. 72
    public override SteeringOutput getSteering(Kinematic selfKinematic, GameObject target = null, Kinematic targetKinematic = null)
    {
        if (target == null)
            return null;

        Vector3 direction = target.transform.position - selfKinematic.position;

        if (direction.sqrMagnitude == 0)
            return null;

        alignTarget.orientation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        return base.getSteering(selfKinematic, target, alignTarget);
    }
        
}
