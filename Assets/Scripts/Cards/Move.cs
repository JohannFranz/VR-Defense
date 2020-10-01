using UnityEngine.UI;
using UnityEngine;
using System;

namespace Card
{
    public class Move : MonoBehaviour
    {
        private float moveVelocity;

        private GameObject moveToPlaceholder;
        private GameObject hand;
        private GameObject deckManager;
        private bool needsMovement;

        private Action movementFinishedCallback;

        // Start is called before the first frame update
        void Awake()
        {
            needsMovement = false;
            hand = GameObject.FindGameObjectWithTag(Constants.HandTag);
            Debug.Assert(hand != null);

            deckManager = GameObject.FindGameObjectWithTag(Constants.DeckManagerTag);
            Debug.Assert(deckManager != null);

            moveVelocity = deckManager.GetComponent<Deck>().drawVelocity;
        }

        // Update is called once per frame
        void Update()
        {
            if (needsMovement == false)
                return;

            Vector3 currentPos = transform.position;
            Vector3 goalPos = moveToPlaceholder.transform.position;
            Vector3 handDirection = goalPos - currentPos;
            handDirection.Normalize();
            Vector3 travelledDistance = handDirection * moveVelocity * Time.deltaTime;

            if ((goalPos - currentPos).magnitude < travelledDistance.magnitude)
            {
                transform.position = goalPos;
                transform.SetParent(hand.transform);
                GetComponent<Image>().raycastTarget = true;
                transform.SetSiblingIndex(moveToPlaceholder.transform.GetSiblingIndex());
                needsMovement = false;
                hand.GetComponent<HandPlacement>().DeactivatePlaceholder(moveToPlaceholder);

                if (movementFinishedCallback != null)
                {
                    movementFinishedCallback();
                    movementFinishedCallback = null;
                }

                return;
            }

            transform.position = currentPos + travelledDistance;
        }

        public void MoveToPlaceholder(GameObject placeholder, Action callback)
        {
            movementFinishedCallback = callback;
            needsMovement = true;
            moveToPlaceholder = placeholder;
            GetComponent<Image>().raycastTarget = false;
        }
    }
}