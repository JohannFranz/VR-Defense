using UnityEngine;

public class AgentController : MonoBehaviour
{
    public float attackRange;
    public Weapon weapon;
    public AgentSensor sensor;
    public GameObject shootTarget;
    public GameObject teamLeader;
    public GameObject goal;

    protected HealthController healthCon;
    protected StateMachine sm;

    void Awake()
    {
        shootTarget = null;

        sm = TD_Utility.Utility.GetChildByTag(gameObject, Constants.StateMachineTag).GetComponent<StateMachine>();
        if (sm == null)
        {
            Debug.LogError("State Machine could not be found for agent: " + gameObject.name);
        }
        sm.SetAgent(gameObject);

        sensor = TD_Utility.Utility.GetChildByTag(gameObject, Constants.SensorTag).GetComponent<AgentSensor>();
        if (sensor == null)
        {
            Debug.LogError("Sensor could not be found for agent: " + gameObject.name);
        }
    }

    void Start()
    {
        if (healthCon == null)
        {
            healthCon = TD_Utility.Utility.GetChildByTag(gameObject, Constants.HealthController).GetComponent<HealthController>();
        }

        weapon.attackRange = attackRange;
    }

    void LateUpdate()
    {
        if (IsDead())
        {
            gameObject.SetActive(false);

            if (gameObject.tag != Constants.MinionTag)
                return;

            TeamController member = TD_Utility.Utility.GetChildByTag(gameObject, Constants.AITag).GetComponent<TeamController>();
            TeamController leader = TD_Utility.Utility.GetChildByTag(teamLeader, Constants.AITag).GetComponent<TeamController>();
            leader.InformAboutDeath(member);
        }
    }

    public void SetAttackRange(float range)
    {
        attackRange = range;
        sensor.SetViewRange(range);
    }

    public HealthController GetHealthController()
    {
        return healthCon;
    }

    public void SetHealthController(HealthController controller)
    {
        healthCon = controller;
    }

    public bool HasTeamLeader()
    {
        return teamLeader != null;
    }

    public bool IsDead()
    {
        return healthCon.GetHealth() <= 0;
    }
}
