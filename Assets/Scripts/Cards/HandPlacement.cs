using UnityEngine.UI;
using UnityEngine;

namespace Card
{
    public class HandPlacement : MonoBehaviour
    {
        private GameObject[] placeholders;

        private Vector2 cardSize;
        private int currentPlaceholderIndex;

        void Awake()
        {
            Deck deck = GameObject.FindGameObjectWithTag(Constants.DeckManager).GetComponent<Deck>();
            Debug.Assert(deck != null);
            cardSize = deck.GetCardSize();

            placeholders = new GameObject[Constants.HandSize];
            InitPlaceHolders();
        }

        private void InitPlaceHolders()
        {
            for (int i = 0; i < Constants.HandSize; i++)
            {
                placeholders[i] = new GameObject("PlaceholderHand " + i.ToString());
                LayoutElement le = placeholders[i].AddComponent<LayoutElement>();
                le.preferredHeight = cardSize.y;
                le.preferredWidth = cardSize.x;
                placeholders[i].transform.SetParent(transform);
                placeholders[i].tag = Constants.PlaceholderTag;
            }
            currentPlaceholderIndex = 0;
        }

        public GameObject GetPlaceholderToReplace()
        {
            if (currentPlaceholderIndex < 0 || currentPlaceholderIndex >= transform.childCount)
                return null;

            currentPlaceholderIndex += 1;
            return placeholders[currentPlaceholderIndex - 1];
        }

        public void DeactivatePlaceholder()
        {
            if (currentPlaceholderIndex < 0 || currentPlaceholderIndex >= transform.childCount)
                return;

            placeholders[currentPlaceholderIndex].SetActive(false);
            placeholders[currentPlaceholderIndex].transform.SetParent(transform.parent);
        }

        public void DeactivatePlaceholder(GameObject placeholder)
        {
            if (placeholder == null)
                return;

            placeholder.SetActive(false);
            placeholder.transform.SetParent(transform.parent);
        }

        public void ActivatePlaceholder(GameObject placeholder, int siblingIndex)
        {
            placeholder.SetActive(true);
            placeholder.transform.SetParent(transform);
            placeholder.transform.SetSiblingIndex(siblingIndex);
        }

        public void RePositionCards(GameObject cardToBeRepositioned)
        {
            GameObject placeholder = cardToBeRepositioned.GetComponent<DragCard>().GetHandReplacement();

            for (int i = placeholder.transform.GetSiblingIndex() + 1; i < transform.childCount; i++)
            {
                Transform childTrans = transform.GetChild(i);
                //unparent all cards to the right of "cardToBeRepositioned"
                int sibling = transform.GetChild(i).GetSiblingIndex();
                if (childTrans.tag == Constants.PlaceholderTag)
                    continue;

                childTrans.GetComponent<Move>().MoveToPlaceholder(placeholders[i - 1], null);
                childTrans.SetParent(transform.parent);
                childTrans.GetComponent<DragCard>().SetHandReplacement(placeholders[i - 1]);
                placeholders[i].transform.SetParent(transform);
                placeholders[i].SetActive(true);
                placeholders[i].transform.SetSiblingIndex(sibling);
            }
            currentPlaceholderIndex -= 1;
        }

        public int GetAmountOfCards()
        {
            int amount = 0;
            if (transform.childCount == 0)
                return amount;

            for (int i = 0; i < Constants.HandSize; i++)
            {
                if (transform.GetChild(i).tag == Constants.PlaceholderTag)
                    continue;

                amount += 1;
            }

            return amount;
        }
    }
}