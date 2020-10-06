using UnityEngine;
using System;

namespace Card
{
    public class DrawCard : MonoBehaviour
    {
        private GameObject card;
        private GameObject hand;
        private GameObject handPlaceholder;
        private VR_Controller vrCon;
        [SerializeField]
        private bool inVRMode;

        private Action drawFinishedCallback;

        // Start is called before the first frame update
        void Awake()
        {
            vrCon = GameObject.Find(Constants.VR_Tag).GetComponent<VR_Controller>();
            inVRMode = vrCon.IsVRMode();
            hand = GameObject.FindGameObjectWithTag(Constants.HandTag);
            Debug.Assert(hand != null);
        }

        //Prepares card for drawing from deck. 
        public void Draw(Action callback)
        {
            //if (inVRMode)
            //{
            //    DrawForVRHand(callback);
            //    return;
            //}

            card = GetComponent<Deck>().GetCardFromTopOfDeck();
            if (card == null)
                return;

            drawFinishedCallback = callback;

            handPlaceholder = hand.GetComponent<HandPlacement>().GetPlaceholderToReplace();
            Debug.Assert(handPlaceholder != null);
            card.GetComponent<DragCard>().SetHandReplacement(handPlaceholder);
            
            if (card.transform.parent.gameObject.tag != Constants.CanvasTag)
                card.transform.SetParent(card.transform.parent);
            card.SetActive(true);

            card.GetComponent<Move>().MoveToPlaceholder(handPlaceholder, drawCallback);
        }

        private void DrawForVRHand(Action callback)
        {
            for (int i = 0; i < hand.transform.childCount; i++)
            {
                Transform childTransform = transform.GetChild(i);
                if (childTransform.tag == Constants.PlaceholderTag)
                {
                    card = GetComponent<Deck>().GetCardFromTopOfDeck();
                    if (card == null)
                        return;

                    drawFinishedCallback = callback;
                    card.GetComponent<DragCard>().SetHandReplacement(childTransform.gameObject);
                    if (card.transform.parent.gameObject.tag != Constants.CanvasTag)
                        card.transform.SetParent(card.transform.parent);
                    card.SetActive(true);

                    card.GetComponent<Move>().MoveToPlaceholder(childTransform.gameObject, drawCallback);
                    return;
                }
            }
        }

        public void drawCallback()
        {
            ActivateCard();
            drawFinishedCallback();
        }

        private void ActivateCard()
        {
            Transform templateTrans = card.transform.Find("Template");
            Debug.Assert(templateTrans != null);
            templateTrans.Find("Front").gameObject.SetActive(true);
            templateTrans.Find("Back").gameObject.SetActive(false);
        }
    }
}