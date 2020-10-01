using UnityEngine;
using UnityEngine.AI;
using TD_Utility;

[CreateAssetMenu(menuName = "AI/States/FormationState")]
public class FormationState : State
{
    public override void EnterState(GameObject agent)
    {
        if (agent.tag == Constants.MinionTag)
            Debug.Log("Enter Formation State");

        Animator anim = agent.GetComponent<Animator>();
        anim.SetBool("isMoving", true);

        GameObject ai = agent.transform.Find("AI").gameObject;
        TeamController teamCon = ai.GetComponent<TeamController>();
        if (teamCon.IsTeamLeader())
        {
            CreateFormation(agent, ai);
        }

        NavMeshAgent navAgent = agent.GetComponent<NavMeshAgent>();
        if (navAgent == null)
            return;
        navAgent.enabled = false;

        KinematicController kin = ai.GetComponent<KinematicController>();
        kin.UpdateOrientation();
        kin.UseKinematic(true);
    }

    public override void ExitState(GameObject agent)
    {
        if (agent.tag == Constants.MinionTag)
            Debug.Log("Exit Formation State");

        GameObject ai = agent.transform.Find("AI").gameObject;
        KinematicController kin = ai.GetComponent<KinematicController>();
        kin.SetTarget(null);
        kin.UseKinematic(false);

        Animator anim = agent.GetComponent<Animator>();
        anim.SetBool("isMoving", false);
    }

    public override void UpdateState(GameObject agent)
    {
        GameObject ai = agent.transform.Find("AI").gameObject;
        TeamController teamCon = ai.GetComponent<TeamController>();
        if (teamCon.IsTeamLeader())
        {
            ReevaluateFormation(teamCon, agent);
        }
    }

    private void CreateFormation(GameObject agent, GameObject ai)
    {
        GameObject formation = Utility.GetChildByTag(ai, Constants.FormationTag);
        FormationController formCon = formation.GetComponent<FormationController>();
        TeamController teamCon = ai.GetComponent<TeamController>();
        foreach (TeamController ally in teamCon.GetTeamMembers())
        {
            formCon.AddFormationMember(ally.gameObject.transform.parent.gameObject);
        }

        AgentController agentCon = agent.GetComponent<AgentController>();
        GameObject nearestEnemy = GetNearestEnemy(agent, ai, agentCon);

        float distanceNearestEnemySqrd = (nearestEnemy.transform.position - agent.transform.position).sqrMagnitude;
        if (distanceNearestEnemySqrd > agentCon.attackRange * agentCon.attackRange)
        {
            Vector3 distToEnemy = nearestEnemy.transform.position - agent.transform.position;
            distToEnemy.y = 0;
            distToEnemy.Normalize();
            Vector3 formationCenter = nearestEnemy.transform.position - distToEnemy * agentCon.attackRange * 0.5f;
            formCon.formation.SetCurrentCenter(agent.transform.position);

            formCon.formation.PlanFormation(formationCenter);
            for (int i = 0; i < formCon.formation.maxCountMembers; i++)
            {
                if (formCon.slots[i].character == null)
                    continue;

                GameObject slotCharacterAI = Utility.GetChildByTag(formCon.slots[i].character, Constants.AITag);
                slotCharacterAI.GetComponent<KinematicController>().SetTarget(formCon.slots[i].seekPosition);
            }
        }
    }

    GameObject GetNearestEnemy(GameObject agent, GameObject ai, AgentController agentCon)
    {
        
        GameObject nearestEnemy = null;
        float distanceNearestEnemySqrd = 0;
        foreach (GameObject enemy in agentCon.sensor.GetEnemies())
        {
            Vector3 distToEnemy = enemy.transform.position - agent.transform.position;
            distToEnemy.y = 0;

            if (nearestEnemy == null)
            {
                nearestEnemy = enemy;
                distanceNearestEnemySqrd = distToEnemy.sqrMagnitude;
                continue;
            }

            if (distanceNearestEnemySqrd > distToEnemy.sqrMagnitude)
            {
                distanceNearestEnemySqrd = distToEnemy.sqrMagnitude;
                nearestEnemy = enemy;
            }
        }
        return nearestEnemy;
    }

    private void ReevaluateFormation(TeamController teamCon, GameObject agent)
    {
        GameObject formationObject = TD_Utility.Utility.GetChildByTag(agent, Constants.FormationTag);
        FormationController formCon = formationObject.GetComponent<FormationController>();
        if (formCon == null)
            return;

        bool hasFormation = false;
        foreach (TeamController member in teamCon.GetTeamMembers())
        {
            if (member.IsTeamLeader())
                continue;

            hasFormation = false;
            foreach (Slot slot in formCon.slots)
            {
                if (slot.character == null)
                    continue;
                if (slot.character.GetInstanceID() == member.transform.parent.gameObject.GetInstanceID())
                {
                    hasFormation = true;
                    break;
                }
            }

            if (hasFormation == false)
                AddToFormation(formCon, member);
        }
    }

    public void AddToFormation(FormationController formCon, TeamController member)
    {
        foreach (Slot slot in formCon.slots)
        {
            if (slot.character != null)
                continue;

            slot.character = member.transform.parent.gameObject;
            break;
        }

        GameObject teamLeader = member.transform.parent.gameObject;
        Vector3 formationPos = formCon.formation.GetCenter();
        Vector3 formationDir = formationPos - teamLeader.transform.position;
        formationDir.Normalize();
        Vector3 newPosition = formationPos + formationDir * 0.1f;
        formCon.formation.PlanFormation(newPosition);

        foreach (Slot slot in formCon.slots)
        {
            if (slot.character == null)
                continue;

            GameObject slotCharacterAI = Utility.GetChildByTag(slot.character, Constants.AITag);
            slotCharacterAI.GetComponent<KinematicController>().SetTarget(slot.seekPosition);
        }
    }
}
