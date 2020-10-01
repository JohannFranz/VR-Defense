using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "AI/States/SeekState")]
public class SeekState : State
{
    public override void EnterState(GameObject agent)
    {
        if (agent.tag == Constants.MinionTag)
            Debug.Log("Enter Seek State");

        

        //NavMeshAgent navAgent = agent.GetComponent<NavMeshAgent>();
        //if (navAgent == null || navAgent.enabled == false)
        //{
        //    GameObject ai = agent.transform.Find("AI").gameObject;
        //    KinematicController kin = ai.GetComponent<KinematicController>();
        //    if (kin == null)
        //    {
        //        Debug.LogError("KinematicController could not be found for agent: " + agent.name);
        //    } else
        //    {
        //        kin.SetTarget(agent.GetComponent<AgentController>().goal);
        //        kin.UseKinematic(true);
        //    }
        //} else
        //{
        //    navAgent.SetDestination(agent.GetComponent<AgentController>().goal.transform.position);
        //}


        //GameObject ai = agent.transform.Find("AI").gameObject;
        //KinematicController kin = ai.GetComponent<KinematicController>();
        //kin.enabled = false;

        NavMeshAgent navAgent = agent.GetComponent<NavMeshAgent>();
        if (navAgent == null)
            return;
        navAgent.enabled = true;
        navAgent.SetDestination(agent.GetComponent<AgentController>().goal.transform.position);

        Animator anim = agent.GetComponent<Animator>();
        anim.SetBool("isMoving", true);
    }

    public override void ExitState(GameObject agent)
    {
        if (agent.tag == Constants.MinionTag)
            Debug.Log("Exit Seek State");

        NavMeshAgent navAgent = agent.GetComponent<NavMeshAgent>();
        if (navAgent == null)
            return;
        navAgent.enabled = false;

        //GameObject ai = agent.transform.Find("AI").gameObject;
        //KinematicController kin = ai.GetComponent<KinematicController>();
        //kin.SetTarget(null);
        //kin.UseKinematic(false);

        //if (navAgent == null || navAgent.isActiveAndEnabled == false)
        //{
        //    GameObject ai = agent.transform.Find("AI").gameObject;
        //    KinematicController kin = ai.GetComponent<KinematicController>();
        //    kin.SetTarget(null);
        //    kin.UseKinematic(false);
        //}
        //else
        //{
        //    navAgent.SetDestination(agent.transform.position);
        //}

        Animator anim = agent.GetComponent<Animator>();
        anim.SetBool("isMoving", false);
    }

    public override void UpdateState(GameObject agent)
    {
        if (agent.tag != Constants.MinionTag)
            return;

        Animator anim = agent.GetComponent<Animator>();
        if (anim.GetBool("isMoving") == false)
            anim.SetBool("isMoving", true);
    }
}
