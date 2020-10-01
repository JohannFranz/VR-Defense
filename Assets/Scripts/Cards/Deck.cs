using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

namespace Card
{
    public class Deck : MonoBehaviour
    {
        public GameObject britishSoldierCard;
        public GameObject germanSoldierCard;
        public GameObject germanSoldierFlamethrowerCard;
        public GameObject specialUnitCard;

        public float drawVelocity;

        private GameObject[] cards;
        private GameObject hand;
        private GameObject deck;

        private Vector2 cardSize;

        private int currentCardIndex;
        private int cardsToDraw;
        private Action cardDrawFinishCallback;


        //static Deck deckInstance;

        // Start is called before the first frame update
        void Awake()
        {
            //if (deckInstance != null)
            //{
            //    Destroy(gameObject);
            //    return;
            //}

            //DontDestroyOnLoad(gameObject);

            hand = GameObject.FindGameObjectWithTag(Constants.HandTag);
            Debug.Assert(hand != null);
            deck = GameObject.FindGameObjectWithTag(Constants.DeckTag);
            Debug.Assert(deck != null);

            InitCards();
        }

        private void InitCards()
        {
            cards = new GameObject[Constants.DeckSize];
            currentCardIndex = 0;
            cardsToDraw = Constants.HandSize;

            //First: create cards
            GameObject[] tempDeck = new GameObject[Constants.DeckSize];

            tempDeck[0] = Instantiate(specialUnitCard, deck.transform.position, Quaternion.identity);
            InitCard(tempDeck[0]);
            for (int i = 1; i < Constants.DeckSize; i++)
            {
                if (i % 3 == 0)
                {
                    tempDeck[i] = Instantiate(britishSoldierCard, deck.transform.position, Quaternion.identity);
                }
                else if (i % 3 == 2)
                {
                    tempDeck[i] = Instantiate(germanSoldierCard, deck.transform.position, Quaternion.identity);
                }
                else if (i % 3 == 1)
                {
                    tempDeck[i] = Instantiate(germanSoldierFlamethrowerCard, deck.transform.position, Quaternion.identity);
                }

                InitCard(tempDeck[i]);
            }

            //Second: shuffle cards for random effect
            ShuffleCards(tempDeck);


            //cards[0].SetActive(true);

            RectTransform recTrans = cards[currentCardIndex].GetComponent<RectTransform>();
            cardSize.y = recTrans.rect.height;
            cardSize.x = recTrans.rect.width;
        }

        private void InitCard(GameObject card)
        {
            CardData data = card.GetComponent<PlayCard>().cardData;
            TD_Utility.Utility.GetChildByTag(card, Constants.DamageTextTag).GetComponent<TextMeshProUGUI>().SetText(data.Damage.ToString());
            TD_Utility.Utility.GetChildByTag(card, Constants.LifeTextTag).GetComponent<TextMeshProUGUI>().SetText(data.Health.ToString());
            TD_Utility.Utility.GetChildByTag(card, Constants.RangeTextTag).GetComponent<TextMeshProUGUI>().SetText(data.AttackRange.ToString());
            TD_Utility.Utility.GetChildByTag(card, Constants.NameTextTag).GetComponent<TextMeshProUGUI>().SetText(data.Name.ToString());

            card.transform.SetParent(deck.transform);
            card.GetComponent<Image>().raycastTarget = false;
            card.SetActive(false);
        }

        private void ShuffleCards(GameObject[] tempDeck)
        {
            //First: create random indices
            int[] indices = new int[Constants.DeckSize];
            for(int i = 0; i < Constants.DeckSize; i++)
            {
                indices[i] = -1;
            }

            int j = 0;
            while(indices[Constants.DeckSize - 1] == -1)
            {
                int randomNumber = (int)(UnityEngine.Random.value * Constants.DeckSize);
                if (TD_Utility.Utility.IsNumberInList(indices, randomNumber))
                    continue;

                indices[j] = randomNumber;
                j++;
            }

            //Second: Switch temp deck with real deck
            for (int i = 0; i < Constants.DeckSize; i++)
            {
                cards[i] = tempDeck[indices[i]];
            }
        }

        public void DrawCards(int amount, Action callback)
        {
            if (amount <= 0)
                return;
            if (amount > Constants.HandSize)
                amount = Constants.HandSize;

            DrawCard drawScript = GetComponent<DrawCard>();
            drawScript.Draw(cardWasDrawn);
            cardDrawFinishCallback = callback;
            cardsToDraw = amount - 1;
        }

        public bool IsHandFullWithCards()
        {
            return cardsToDraw <= 0;
        }

        public void cardWasDrawn()
        {
            if (cardsToDraw <= 0)
            {
                cardDrawFinishCallback();
                return;
            }

            cardsToDraw -= 1;
            DrawCard drawScript = GetComponent<DrawCard>();
            drawScript.Draw(cardWasDrawn);
        }

        public bool AddCardToDeck()
        {

            return true;
        }

        public bool RemoveCardFromDeck()
        {

            return true;
        }

        public GameObject GetCardFromTopOfDeck()
        {
            if (currentCardIndex >= cards.Length) return null;

            currentCardIndex += 1;
            //set the next card on the deck active, so the top card of the deck is always visible
            if (currentCardIndex < cards.Length)
                cards[currentCardIndex].SetActive(true);

            return cards[currentCardIndex - 1];
        }

        public Vector2 GetCardSize()
        {
            return cardSize;
        }



        //public static Deck GetDeckInstance()
        //{
        //    return deckInstance;
        //}
    }
}