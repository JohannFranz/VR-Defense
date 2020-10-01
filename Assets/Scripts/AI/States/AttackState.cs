using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[CreateAssetMenu(menuName = "AI/States/AttackState")]
public class AttackState : State
{
    public override void EnterState(GameObject agent)
    {
        if (agent.tag == Constants.MinionTag)
            Debug.Log("Enter Attack State");

        AgentController controller = agent.GetComponent<AgentController>();

        //Get target from sensor
        SetNextTarget(agent, controller);

        Animator anim = agent.GetComponent<Animator>();
        anim.SetBool("hasTargetInRange", true);

        //NavMeshAgent navAgent = agent.GetComponent<NavMeshAgent>();
        //if (navAgent == null)
        //    return;
        //navAgent.enabled = false;
    }

    public override void ExitState(GameObject agent)
    {
        if (agent.tag == Constants.MinionTag)
            Debug.Log("Exit Attack State");

        agent.GetComponent<AgentController>().shootTarget = null;

        Animator anim = agent.GetComponent<Animator>();
        anim.SetBool("hasTargetInRange", false);
    }

    public override void UpdateState(GameObject agent)
    {
        AgentController controller = agent.GetComponent<AgentController>();
        if (controller.shootTarget == null)
            return;

        if (controller.shootTarget.GetComponent<AgentController>().IsDead())
            SetNextTarget(agent, controller);

        if (IsFacingTarget(agent) == false)
            return;

        if (IsWeaponReady(agent) == false)
            return;
        
        controller.weapon.Attack();
    }

    private bool IsFacingTarget(GameObject agent)
    {
        Vector3 lookDir = agent.transform.forward;
        lookDir.Normalize();

        Vector3 targetDir = agent.GetComponent<AgentController>().shootTarget.transform.position - agent.transform.position;
        float distance = targetDir.magnitude;
        targetDir.Normalize();
        float dot = Vector3.Dot(lookDir, targetDir);

        return dot > 0.99f;
    }

    private bool IsWeaponReady(GameObject agent)
    {
        Animator anim = agent.GetComponent<Animator>();

        return anim.GetCurrentAnimatorStateInfo(0).IsName("Shoot");
    }

    private GameObject ChooseTarget(GameObject agent, AgentController controller, List<GameObject> enemies)
    {
        GameObject target = null;
        float targetDot = -1.0f;
        float targetDist = 9999.0f;
        Vector3 lookDir, targetDir;

        lookDir = agent.transform.forward;
        lookDir.Normalize();

        //choose the target that is in front of the minion to reduce rotation time
        //among all targets in fron of the minion choose the closest one
        foreach (GameObject enemy in enemies)
        {
            Vector3 enemyDir = enemy.transform.position - agent.transform.position;
            if (IsTargetOutOfRange(agent, controller, enemyDir.magnitude))
                continue;

            targetDir = enemy.transform.position - agent.transform.position;
            float distance = targetDir.magnitude;
            targetDir.Normalize();
            float dot = Vector3.Dot(lookDir, targetDir);

            if (target == null)
            {
                target = enemy;
                targetDot = dot;
                targetDist = distance;
                continue;
            }

            //compare distance and dot values
            if (distance < targetDist)
            {
                if (dot >= Constants.TargetChooseDotValue)
                {
                    target = enemy;
                    targetDot = dot;
                    targetDist = distance;
                }
            }
            else
            {
                if (targetDot < Constants.TargetChooseDotValue)
                {
                    target = enemy;
                    targetDot = dot;
                    targetDist = distance;
                }
            }
        }

        return target;
    }

    private bool IsTargetOutOfRange(GameObject agent, AgentController controller, float length)
    {
        return length > controller.attackRange;
    }

    private bool IsTargetOutOfRange(GameObject agent, AgentController controller)
    {
        Vector3 targetPos = controller.shootTarget.transform.position;
        Vector3 agentPos = agent.transform.position;
        float distance = (targetPos - agentPos).magnitude;
        return distance > controller.attackRange;
    }

    public void SetNextTarget(GameObject agent, AgentController controller)
    {
        AgentSensor sense = controller.sensor;
        List<GameObject> enemies = sense.GetEnemies();
        controller.shootTarget = ChooseTarget(agent, controller, enemies);
        controller.weapon.SetTarget(controller.shootTarget);

        GameObject aiChild = TD_Utility.Utility.GetChildByTag(agent, Constants.AITag);
        Face steering = TD_Utility.Utility.GetChildByTag(aiChild, Constants.SteeringTag).GetComponent<Face>();
        KinematicController kin = aiChild.GetComponent<KinematicController>();
        kin.SetAlternativeSteeringBehaviour(steering);
        kin.SetTarget(controller.shootTarget);
        kin.UseKinematic(true);
    }
}
