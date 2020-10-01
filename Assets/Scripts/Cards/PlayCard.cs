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

        void Start()
        {
            dragScript = GetComponent<DragCard>();

            //Instantiate agent belonging to card
            Factory.AgentFactory af = GameObject.FindWithTag(Constants.FactoryTag).GetComponent<Factory.AgentFactory>();
            mercenary = af.CreateMercenary(cardData.type, Vector3.zero, cardData.Health, cardData.AttackRange);
            mercenaryRenderer = mercenary.GetComponentsInChildren<MeshRenderer>();
            mercenary.GetComponent<AgentController>().weapon.SetDamage(cardData.Damage);
            mercenary.SetActive(false);

            cam = GameObject.FindWithTag(Constants.CameraTag).GetComponent<Camera>();
            canGroup = GetComponent<CanvasGroup>();
            hand = GameObject.FindGameObjectWithTag(Constants.HandTag);
        }

        public void OnPointerDown()
        {
            currentMousePosition = Input.mousePosition;

            mercenary.transform.position = GetPositionForAgent();
            ProcessMercenaryVisualEffects();
        }

        public void OnPointerUp()
        {
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
            currentMousePosition = Input.mousePosition;
            mercenary.transform.position = GetPositionForAgent();
            ProcessMercenaryVisualEffects();
        }

        private Vector3 GetPositionForAgent()
        {
            RaycastHit hit;
            Ray camRay = cam.ScreenPointToRay(currentMousePosition);
            bool wasHit = Physics.Raycast(camRay.origin, camRay.direction, out hit, 100, Physics.DefaultRaycastLayers);
            //Debug.DrawRay(camRay.origin, camRay.direction * 1000);


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
            Ray camRay = cam.ScreenPointToRay(currentMousePosition);
            bool wasHit = Physics.Raycast(camRay.origin, camRay.direction, out hit, 100, Physics.DefaultRaycastLayers);

            if (wasHit == false)
                return false;

            if (hit.transform.gameObject.CompareTag(Constants.PlacementTag) == false)
                return false;

            placementArea = hit.transform.gameObject;
            if (placementArea.GetComponent<Placement>().IsAlreadyOccupied())
                return false;

            return true;
        }
    }
}