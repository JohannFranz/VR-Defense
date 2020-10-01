using UnityEngine;
using System;

namespace Card
{
    public class DrawCard : MonoBehaviour
    {
        private GameObject card;
        private GameObject hand;
        private GameObject handPlaceholder;

        private Action drawFinishedCallback;

        // Start is called before the first frame update
        void Awake()
        {
            hand = GameObject.FindGameObjectWithTag(Constants.HandTag);
            Debug.Assert(hand != null);
        }

        //Prepares card for drawing from deck. 
        public void Draw(Action callback)
        {
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