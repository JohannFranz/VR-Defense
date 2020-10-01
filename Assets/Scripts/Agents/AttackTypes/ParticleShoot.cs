using UnityEngine;

public class ParticleShoot : MonoBehaviour
{
    public float timeTillFirstHit = 1;

    private float timeSinceAttackStart;
    private float damagePerSecond;
    private float attackRange;
    private GameObject target;
    private Vector3 origin;
    private Vector3 direction;

    private bool startFire;

    // Start is called before the first frame update
    void Start()
    {
        timeSinceAttackStart = 0;
        target = null;
        damagePerSecond = GetComponent<ParticleWeapon>().damagePerSecond;
        attackRange = GetComponent<ParticleWeapon>().attackRange;
    }

    // Update is called once per frame
    void Update()
    {
        if (startFire == false)
            return;

        timeSinceAttackStart += Time.deltaTime;

        if (timeSinceAttackStart < timeTillFirstHit)
            return;

        if (target == null)
        {
            RaycastHit hit;
            if (Physics.Raycast(origin, direction, out hit, attackRange) == false)
                return;

            target = hit.rigidbody.gameObject;
        }

        AgentController ac = target.GetComponent<AgentController>();
        HealthController hc = ac.GetHealthController();
        hc.ReduceHealth(damagePerSecond * Time.deltaTime);
    }

    public void Fire(Vector3 direction, Vector3 origin)
    {
        target = null;
        timeSinceAttackStart = 0;
        this.origin = origin;
        this.direction = direction;
        startFire = true;
    }
}
