using UnityEngine;

public class Wave : MonoBehaviour
{
    enum WaveState
    {
        sleeping = 0,
        spawning = 1,
        progressing = 2,
        finished = 3
    }

    public WaveData wave;

    private float minionHealth;
    private int amount;
    private float interval;
    private Factory.MinionType type;

    private GameObject[] minions;
    private WaveState state;
    private GameObject spawnPosition;
    private GameObject goal;

    private float timeTillSpawn;
    private int spawnedMinions;

    private SpawnController parentCon;


    void Start()
    {
        InitWaveData();

        state = WaveState.sleeping;
        timeTillSpawn = 0.0f;
        spawnedMinions = 0;

        parentCon = transform.parent.gameObject.GetComponent<SpawnController>();
    }

    void Update()
    {
        if (state == WaveState.finished || state == WaveState.sleeping)
            return;

        if (state == WaveState.progressing)
        {
            if (AreMinionsLeft())
            {
                state = WaveState.finished;
            }
        }
        else if (state == WaveState.spawning)
        {
            Spawn();
        }
    }

    private void InitWaveData()
    {
        Factory.AgentFactory af = GameObject.FindWithTag(Constants.FactoryTag).GetComponent<Factory.AgentFactory>();
        amount = wave.amount;
        interval = wave.interval;
        minionHealth = wave.health;
        minions = new GameObject[amount];

        for (int i = 0; i < amount; i++)
        {
            minions[i] = af.CreateMinion(wave.type, Vector3.zero, minionHealth, wave.attackRange);
            minions[i].SetActive(false);
            minions[i].GetComponent<AgentController>().weapon.SetDamage(wave.damage);

            //set health
            HealthController ht = minions[i].transform.Find("HealthController").gameObject.GetComponent<HealthController>();
            ht.Init(wave.health);
        }
    }

    public void Activate(GameObject spawnPos, GameObject goal)
    {
        if (state != WaveState.sleeping) 
        {
            Debug.LogError("Wrong state. Wave is in state:" + state.ToString());
            return;
        }

        this.goal = goal;
        spawnPosition = spawnPos;
        state = WaveState.spawning;
    }

    private bool AreMinionsLeft()
    {
        for (int i = 0; i < minions.Length; i++)
        {
            if (minions[i].activeSelf)
                return false;
        }

        return true;
    }

    public bool IsWaveFinished()
    {
        return state == WaveState.finished;
    }


    public GameObject[] GetMinions()
    {
        return minions;
    }

    private void Spawn()
    {
        if (timeTillSpawn <= 0.0f)
        {
            minions[spawnedMinions].transform.position = spawnPosition.transform.position;
            minions[spawnedMinions].SetActive(true);

            //set goal
            minions[spawnedMinions].GetComponent<MinionController>().goal = goal;

            spawnedMinions += 1;
            timeTillSpawn = interval;

            if (spawnedMinions >= amount)
                state = WaveState.progressing;

            return;
        }

        timeTillSpawn -= Time.deltaTime;
    }

    public bool IsSleeping()
    {
        return state == WaveState.sleeping;
    }

    public bool IsFinished()
    {
        return state == WaveState.finished;
    }
}
