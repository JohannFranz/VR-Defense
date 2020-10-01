using System.Collections.Generic;
using UnityEngine;

public class AgentSensor : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> allies;
    [SerializeField]
    private List<GameObject> enemies;
    //enemies that are attacking the agent but they are out of range
    private List<GameObject> hiddenEnemies;

    private float attackRange;
    private string enemyTag;
    private string allyTag;

    private TeamController teamCon;

    private void Awake()
    {
        allies = new List<GameObject>();
        enemies = new List<GameObject>();
        hiddenEnemies = new List<GameObject>();

        allyTag = transform.parent.gameObject.tag;
        if (allyTag == Constants.MinionTag)
        {
            enemyTag = Constants.MercenaryTag;
        }
        else
        {
            enemyTag = Constants.MinionTag;
        }

        teamCon = transform.parent.Find("AI").gameObject.GetComponent<TeamController>();
    }

    void Start()
    {
        allies.Clear();
        enemies.Clear();
        hiddenEnemies.Clear();
    }

    void Update()
    {
        Reevaluate();
    }

    public void SetViewRange(float range)
    {
        attackRange = range;
        GetComponent<SphereCollider>().radius = range * 2;  // 2 times attack range
    }

    public List<GameObject> GetAllies()
    {
        return allies;
    }

    public List<GameObject> GetEnemies()
    {
        return enemies;
    }

    public List<GameObject> GetHiddenEnemies()
    {
        return hiddenEnemies;
    }

    //checks the enemies-list if all enemies are still in range and alive
    public void Reevaluate()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].activeSelf == false)
            {
                enemies.Remove(enemies[i]);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == enemyTag )
        {
            if (enemies.Contains(other.gameObject) == true)
                return;

            enemies.Add(other.gameObject);
        } else if (other.tag == allyTag)
        {
            if (allies.Contains(other.gameObject) == true)
                return;

            allies.Add(other.gameObject);
            teamCon.CheckAllyInRange(other.transform.Find("AI").GetComponent<TeamController>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == enemyTag)
        {
            enemies.Remove(other.gameObject);
        } else if (other.tag == allyTag)
        {
            allies.Remove(other.gameObject);
        }
    }
}
