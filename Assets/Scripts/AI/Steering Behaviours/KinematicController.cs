using UnityEngine;

public class KinematicController : MonoBehaviour
{
    public float maxVelocity;
    public float maxAcceleration;
    public float maxRotation;
    public float maxAngularAcceleration;
    public GameObject target;

    private Kinematic selfKinematic;
    private Kinematic targetKinematic;
    private Transform parentTransform;
    private CombinedSteeringBehaviour behaviour;
    private Animator anim;
    private Vector3 formerPosition;
    private SteeringBehaviour singleAlternative;
    private bool useKinematic;

    void Awake()
    {
        parentTransform = transform.parent.gameObject.transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        selfKinematic = new Kinematic();
        selfKinematic.maxVelocity = maxVelocity;
        selfKinematic.maxAcceleration = maxAcceleration;
        selfKinematic.maxVelocitySquared = maxVelocity * maxVelocity;
        selfKinematic.maxAccelerationSquared = maxAcceleration * maxAcceleration;
        selfKinematic.maxRotation = maxRotation;
        selfKinematic.maxAngularAcceleration = maxAngularAcceleration;

        GameObject steering = gameObject.transform.Find("Steering").gameObject;
        if (steering == null)
            Debug.Log("ist null für " + name);
        behaviour = gameObject.transform.Find("Steering").GetComponent<CombinedSteeringBehaviour>();
        anim = transform.parent.gameObject.GetComponent<Animator>();

        singleAlternative = null;
        useKinematic = false;
    }
        
    // Update is called once per frame
    void Update()
    {
        if (useKinematic == false)
            return;

        SteeringOutput steering;
        if (singleAlternative == null)
        {
            steering = behaviour.getSteering(selfKinematic, target, targetKinematic);
        } else
        {
            steering = singleAlternative.getSteering(selfKinematic, target, targetKinematic);
        }

        if (target != null)
            Debug.DrawRay(parentTransform.position, target.transform.position - parentTransform.position, Color.white);

        ProcessSteering();
        UpdateSelfKinematic(steering);
    }

    private void ProcessSteering()
    {
        parentTransform.position += selfKinematic.velocity * Time.deltaTime;
        selfKinematic.position = parentTransform.position;
        selfKinematic.orientation += selfKinematic.rotation * Time.deltaTime;
        parentTransform.Rotate(Vector3.up, selfKinematic.rotation * Time.deltaTime);
    }

    private void UpdateSelfKinematic(SteeringOutput steering)
    {
        if (steering != null)
        {
            selfKinematic.rotation += steering.angular * Time.deltaTime;
            if (steering.linear.magnitude > 0)
            {
                selfKinematic.velocity += steering.linear * Time.deltaTime;
                anim.SetBool("isMoving", true);
            }
            else
            {
                selfKinematic.velocity = Vector3.zero;
                anim.SetBool("isMoving", false);
            }
        }
        else
        {
            selfKinematic.velocity = Vector3.zero;
            anim.SetBool("isMoving", false);
        }

        if (selfKinematic.velocity.sqrMagnitude > selfKinematic.maxVelocitySquared)
        {
            selfKinematic.velocity.Normalize();
            selfKinematic.velocity *= selfKinematic.maxVelocity;
        }
    }

    public Kinematic GetKinematic()
    {
        return selfKinematic;
    }

    public void SetTarget(GameObject target)
    {
        if (target == null)
            return;

        this.target = target;
        KinematicController con = target.GetComponent<KinematicController>();
        if (con == null)
            return;
        targetKinematic = con.GetKinematic();
    }

    public void SetTarget(Vector3 targetPos)
    {
        target = new GameObject();
        target.transform.position = targetPos;
        targetKinematic = null;
    }

    public void UpdateOrientation()
    {
        selfKinematic.orientation = Mathf.Atan2(parentTransform.forward.x, parentTransform.forward.z) * Mathf.Rad2Deg;
    }

    public void UseKinematic(bool use)
    {
        useKinematic = use;
    }

    public bool HasReachedTarget()
    {
        if (target == null)
            return false;

        Vector3 distance = target.transform.position - parentTransform.position;
        distance.y = 0;
        return distance.sqrMagnitude < 2;
    }

    public void SetAlternativeSteeringBehaviour(SteeringBehaviour behaviour)
    {
        singleAlternative = behaviour;
    }
}
