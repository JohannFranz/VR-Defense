using UnityEngine;

namespace Card
{
    public class PlayCard : MonoBehaviour
    {
        public CardData cardData;

        private DragCard dragScript;
        private Vector3 currentMousePosition;
        private GameObject placementArea;

        private GameObject mercenary;
        private Factory.MercenaryType type;
        private MeshRenderer[] mercenaryRenderer;

        private Camera cam;
        private CanvasGroup canGroup;
        private GameObject hand;

        //private VR_Controller vrcon;
        private bool inVRMode;

        void Start()
        {
            InitMercenary();

            VR_Controller vrCon = GameObject.Find(Constants.VR_Tag).GetComponent<VR_Controller>();
            if (vrCon.IsVRMode())
            {
                inVRMode = true;
                return;
            }

            dragScript = GetComponent<DragCard>();
            canGroup = GetComponent<CanvasGroup>();
            hand = GameObject.FindGameObjectWithTag(Constants.HandTag);
            cam = GameObject.FindWithTag(Constants.CameraTag).GetComponent<Camera>();
        }

        private void InitMercenary()
        {
            //Instantiate agent belonging to card
            Factory.AgentFactory af = GameObject.FindWithTag(Constants.FactoryTag).GetComponent<Factory.AgentFactory>();
            mercenary = af.CreateMercenary(cardData.type, Vector3.zero, cardData.Health, cardData.AttackRange);
            mercenaryRenderer = mercenary.GetComponentsInChildren<MeshRenderer>();
            mercenary.GetComponent<AgentController>().weapon.SetDamage(cardData.Damage);
            mercenary.SetActive(false);
        }

        public void OnPointerDown()
        {
            if (inVRMode)
                return;

            currentMousePosition = Input.mousePosition;

            mercenary.transform.position = GetPositionForAgent();
            ProcessMercenaryVisualEffects();
        }

        public void OnPointerUp()
        {
            if (inVRMode)
                return;

            if (WasValidPlacementAreaSelected())
            {
                mercenary.transform.position = placementArea.transform.position;
                placementArea.GetComponent<Placement>().Occupy(mercenary);
                hand.GetComponent<HandPlacement>().RePositionCards(gameObject);
                Destroy(gameObject);
            }
            else
            {
                mercenary.SetActive(false);
                dragScript.PrepareReturnToHand();
            }
        }

        public void OnDrag()
        {
            if (inVRMode)
                return;

            currentMousePosition = Input.mousePosition;
            mercenary.transform.position = GetPositionForAgent();
            ProcessMercenaryVisualEffects();
        }

        private Vector3 GetPositionForAgent()
        {
            RaycastHit hit;
            Ray ray;
            ray = cam.ScreenPointToRay(currentMousePosition);
            
            bool wasHit = Physics.Raycast(ray.origin, ray.direction, out hit, 100, Constants.GroundLayer);
            Debug.DrawRay(ray.origin, ray.direction * 1000);

            return hit.point;
        }

        private void ProcessMercenaryVisualEffects()
        {
            Color old = mercenaryRenderer[0].material.color;
            old.a = 1.0f - canGroup.alpha;
            mercenaryRenderer[0].material.color = old;

            if (old.a < 0.5f)
            {
                mercenary.SetActive(false);
            }
            else
            {
                mercenary.SetActive(true);
            }
        }

        private bool WasValidPlacementAreaSelected()
        {
            RaycastHit hit;
            Ray ray;
            //if (vrcon.IsVRMode())
            //{
            //    ray = vrcon.GetPointerRay();
            //}
            //else
            //{
                ray = cam.ScreenPointToRay(currentMousePosition);
            //}
            bool wasHit = Physics.Raycast(ray.origin, ray.direction, out hit, 100, Constants.GroundLayer);

            if (wasHit == false)
                return false;

            if (hit.transform.gameObject.CompareTag(Constants.PlacementTag) == false)
                return false;

            placementArea = hit.transform.gameObject;
            if (placementArea.GetComponent<Placement>().IsAlreadyOccupied())
                return false;

            return true;
        }

        public GameObject GetMercenary()
        {
            return mercenary;
        }
    }
}