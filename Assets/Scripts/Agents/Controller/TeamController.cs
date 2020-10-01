using System.Collections.Generic;
using UnityEngine;

public class TeamController : MonoBehaviour
{
    [SerializeField]
    private bool isTeamLeader;
    [SerializeField]
    private List<TeamController> teamMembers;
    [SerializeField]
    private TeamController teamLeader;

    private AgentController agentCon;
    private FormationController formCon;

    void Awake()
    {
        teamMembers = new List<TeamController>();
        agentCon = transform.parent.gameObject.GetComponent<AgentController>();
        Debug.Assert(agentCon != null);

        if (agentCon.gameObject.tag == Constants.MercenaryTag)
            return;

        GameObject formationObject = TD_Utility.Utility.GetChildByTag(gameObject, Constants.FormationTag);
        formCon = formationObject.GetComponent<FormationController>();
        Debug.Assert(formCon != null);
    }

    void Start()
    {
        isTeamLeader = false;
        teamLeader = null;
        teamMembers.Clear();
    }

    public bool IsTeamLeader()
    {
        return isTeamLeader;
    }

    public void SetTeamLeader(TeamController leader)
    {
        leader.isTeamLeader = true;
        teamLeader = leader;
        GameObject leaderObject = leader.transform.parent.gameObject;
        agentCon.teamLeader = leaderObject;
        if (teamMembers.Contains(this) == false)
            teamMembers.Add(this);
    }

    public bool IsInTeam()
    {
        return teamLeader != null;
    }

    public bool IsPartOfFormation(GameObject agent)
    {
        
        if (formCon == null)
            return false;

        foreach (Slot slot in formCon.slots)
        {
            if (slot.character == null)
                continue;
            if (slot.character.GetInstanceID() == agent.GetInstanceID())
                return true;
        }

        return false;
    }


    public void InformAboutDeath(TeamController member)
    {
        if (member.agentCon.gameObject.tag == Constants.MercenaryTag)
            return;

        member.formCon.RemoveFromFormation(member.agentCon.gameObject);
        member.formCon.UpdateFormation();

        if (member.isTeamLeader && teamMembers.Count > 1)
        {
            NominateNextTeamleader();
            TransferFormation(teamLeader);
        }
    }

    public List<TeamController> GetTeamMembers()
    {
        return teamMembers;
    }

    public TeamController GetTeamLeader()
    {
        return teamLeader;
    }

    public void Recruit(TeamController teamLeader)
    {
        this.teamLeader = teamLeader;
    }

    public void JoinTeam(TeamController member)
    {
        if (teamLeader == null)
            return;
        if (isTeamLeader == false)
        {
            teamLeader.JoinTeam(member);
            return;
        }

        if (teamMembers.Contains(member))
            return;

        teamMembers.Add(member);
    }

    //returns true if a team was found
    public bool LookForTeam(AgentSensor sensor)
    {
        //1. Find all nearby allies 
        FindTeamMembers(sensor);

        //2. check if any ally is already a team leader or is in a team
        foreach (TeamController controller in teamMembers)
        {
            if (controller.teamLeader != null)
            {
                JoinTeam(this);
                return true;
            }
        }

        return false;
    }

    public void CreateTeam(AgentSensor sensor)
    {
        SetTeamLeader(this);
        FindTeamMembers(sensor);

        foreach(TeamController controller in teamMembers)
        {
            controller.SetTeamLeader(this);
        }
    }

    private void FindTeamMembers(AgentSensor sensor)
    {
        if (teamMembers.Count != 0)
            ResetTeam();

        List<GameObject> allies = sensor.GetAllies();
        foreach (GameObject go in allies)
        {
            GameObject child = TD_Utility.Utility.GetChildByTag(go, Constants.TeamTag);
            TeamController controller = child.GetComponent<TeamController>();
            teamMembers.Add(controller);
        }
    }

    private void ResetTeam()
    {
        foreach (TeamController controller in teamMembers)
        {
            controller.teamLeader = null;
        }
        teamMembers.Clear();
    }

    public void CheckAllyInRange(TeamController ally)
    {
        if (isTeamLeader == false && teamLeader == null)
        {
            if (ally.IsInTeam())
            {
                ally.JoinTeam(this);
                return;
            }
            SetTeamLeader(this);
            ally.SetTeamLeader(this);
            JoinTeam(ally);
        } else
        {
            ally.SetTeamLeader(teamLeader);
            JoinTeam(ally);
        }
    }

    public void NominateNextTeamleader()
    {
        isTeamLeader = false;
        TeamController newTeamLeader = null;
        for (int i = 0; i < teamMembers.Count; i++)
        {
            if (teamMembers[i] == this)
                continue;

            if (newTeamLeader == null)
                newTeamLeader = teamMembers[i];

            teamMembers[i].SetTeamLeader(newTeamLeader);
        }

        if (teamMembers == null)
            return;

        teamMembers.Remove(this);
        teamLeader = newTeamLeader;

        foreach (TeamController member in teamMembers)
        {
            if (newTeamLeader.teamMembers.Contains(member))
                continue;

            newTeamLeader.teamMembers.Add(member);
        }
    }

    private void TransferFormation(TeamController teamCon)
    {
        formCon.TransferFormationTo(teamCon.formCon);
    }

    public bool IsInFront(GameObject agent)
    {
        return formCon.IsInFront(agent);
    }

}
