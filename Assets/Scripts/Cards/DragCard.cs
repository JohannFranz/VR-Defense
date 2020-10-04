using UnityEngine;


namespace Card
{
    public class DragCard : MonoBehaviour
    {
        private Vector3 lastMousePosition;
        private Vector3 currentMousePosition;

        private int initialPositionInHand;
        private Vector3 initialPosition;

        private CanvasGroup canGroup;
        private Vector2 cardSize;

        private GameObject hand;
        private GameObject handPlaceholder;

        private bool inVRMode;

        void Start()
        {
            VR_Controller vrCon = GameObject.Find(Constants.VR_Tag).GetComponent<VR_Controller>();
            if (vrCon.IsVRMode())
            {
                //enabled = false;
                inVRMode = true;
                return;
            }

            inVRMode = false;

            canGroup = GetComponent<CanvasGroup>();

            cardSize = GameObject.FindGameObjectWithTag(Constants.DeckManager).GetComponent<Deck>().GetCardSize();
            hand = GameObject.FindGameObjectWithTag(Constants.HandTag);
        }

        public void OnPointerDown()
        {
            if (inVRMode)
                return;

            if (transform.parent.tag != Constants.HandTag)
            {
                Debug.LogError("card not in Hand.");
                return;
            }

            initialPosition = transform.position;
            lastMousePosition = Input.mousePosition;
            initialPositionInHand = transform.GetSiblingIndex();
            transform.SetParent(transform.parent.parent);
            hand.GetComponent<HandPlacement>().ActivatePlaceholder(handPlaceholder, initialPositionInHand);
        }

        public void OnPointerUp()
        {
            if (inVRMode)
                return;

            canGroup.alpha = 1.0f;
            lastMousePosition = Constants.NullVector3;
        }

        public void OnDrag()
        {
            if (inVRMode)
                return;

            if (lastMousePosition == Constants.NullVector3)
            {
                Debug.LogError("lastMousePosition is set to NullVector3.");
            }

            currentMousePosition = Input.mousePosition;
            MoveCard();
            ProcessVisualEffectsOnCard();

            lastMousePosition = currentMousePosition;
        }

        public void PrepareReturnToHand()
        {
            GetComponent<Move>().MoveToPlaceholder(handPlaceholder, null);
        }

        private void MoveCard()
        {
            Vector3 newPosition = transform.position + (currentMousePosition - lastMousePosition);
            newPosition = HandleCardOutOfBounds(newPosition);
            transform.position = newPosition;
        }

        private Vector3 HandleCardOutOfBounds(Vector3 newPosition)
        {
            float verticalOffset = cardSize.y * 0.5f;
            float horizontalOffset = cardSize.x * 0.5f;

            if (newPosition.y < verticalOffset) newPosition.y = verticalOffset;
            if (newPosition.x < horizontalOffset) newPosition.x = horizontalOffset;
            if (newPosition.x > (Screen.width - horizontalOffset)) newPosition.x = (Screen.width - horizontalOffset);

            return newPosition;
        }

        private void ProcessVisualEffectsOnCard()
        {
            float verticalDistance = transform.position.y - initialPosition.y;
            canGroup.alpha = 1 - verticalDistance / cardSize.y;
        }

        public void SetHandReplacement(GameObject replacement)
        {
            handPlaceholder = replacement;
        }

        public GameObject GetHandReplacement()
        {
            return handPlaceholder;
        }
    }
}
