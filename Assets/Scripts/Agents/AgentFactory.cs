using UnityEngine;

namespace Factory
{
    public enum AgentType
    {
        Minion = 0,
        Mercenary = 1
    }

    public enum MinionType
    {
        BlackRobot,
        GreenRobot,
        BlueRobot,
        WhiteRobot
    }

    public enum MercenaryType
    {
        BritishSoldier,
        GermanSoldier,
        GermanFlamethrower,
        SpecialUnit
    }

    public class AgentFactory : MonoBehaviour
    {
        static AgentFactory factoryInstance;

        public Transform cam;

        [Header("Mercenaries")]
        public GameObject BritishSoldier;
        public GameObject GermanSoldier;
        public GameObject GermanFlamethrower;
        public GameObject SpecialUnit;

        [Header("Minions")]
        public GameObject BlackRobot;
        public GameObject GreenRobot;
        public GameObject BlueRobot;
        public GameObject WhiteRobot;

        void Awake()
        {
            if (factoryInstance != null)
            {
                Destroy(gameObject);
                return;
            }

            factoryInstance = this;
            DontDestroyOnLoad(gameObject);
        }

        public GameObject CreateMercenary(MercenaryType type, Vector3 position, float health, float range)
        {
            if (type == MercenaryType.BritishSoldier) return InitAgent(BritishSoldier, position, health, range);
            if (type == MercenaryType.GermanSoldier) return InitAgent(GermanSoldier, position, health, range);
            if (type == MercenaryType.GermanFlamethrower) return InitAgent(GermanFlamethrower, position, health, range);
            if (type == MercenaryType.SpecialUnit) return InitAgent(SpecialUnit, position, health, range);

            return null;
        }

        public GameObject CreateMinion(MinionType type, Vector3 position, float health, float range)
        {
            if (type == MinionType.BlackRobot) return InitAgent(BlackRobot, position, health, range);
            if (type == MinionType.GreenRobot) return InitAgent(GreenRobot, position, health, range);
            if (type == MinionType.BlueRobot) return InitAgent(BlueRobot, position, health, range);
            if (type == MinionType.WhiteRobot) return InitAgent(WhiteRobot, position, health, range);

            return null;
        }

        private GameObject InitAgent(GameObject agent, Vector3 position, float health, float range)
        {
            GameObject newAgent = Instantiate(agent, position, Quaternion.identity);

            newAgent.GetComponent<AgentController>().SetAttackRange(range);

            //initialize Healthbar
            GameObject controller = TD_Utility.Utility.GetChildByTag(newAgent, Constants.HealthControllerTag);
            HealthController healthcontroller = controller.GetComponent<HealthController>();
            healthcontroller.Init(health, newAgent, cam);
            newAgent.GetComponent<AgentController>().SetHealthController(healthcontroller);
            return newAgent;
        }
    }
}

