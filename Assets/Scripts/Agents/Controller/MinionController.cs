using System.Collections.Generic;
using UnityEngine;

public class MinionController : AgentController
{
    private float timeSinceStart;

    void Start()
    {
        timeSinceStart = 0;
    }

    void Update()
    {
        timeSinceStart += Time.deltaTime;

        if (timeSinceStart > 1)
        {
            if (teamLeader == null)
            {
                GameObject ai = TD_Utility.Utility.GetChildByTag(gameObject, Constants.AITag);
                TeamController teamCon = ai.GetComponent<TeamController>();
                teamCon.SetTeamLeader(teamCon);
            }
        }
    }

}
