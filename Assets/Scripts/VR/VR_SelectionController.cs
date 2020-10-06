using System.Collections.Generic;
using UnityEngine;

public class VR_SelectionController : MonoBehaviour
{
    [SerializeField]
    private bool inVRMode;

    private VR_Controller vrCon;

    private List<VRPlacementSelector> placements;
    private List<Card.VRCardSelector> cards;
    private GameObject hand;

    // Start is called before the first frame update
    void Start()
    {
        placements = new List<VRPlacementSelector>();
        cards = new List<Card.VRCardSelector>();
        vrCon = GameObject.Find(Constants.VR_Tag).GetComponent<VR_Controller>();
        hand = GameObject.FindGameObjectWithTag(Constants.HandTag);
        inVRMode = vrCon.IsVRMode();

        FindPlacements();
        FindCards();
    }

    // Update is called once per frame
    void Update()
    {
        VRPlacementSelector selectedPlacement = GetSelectedPlacement();
        Card.VRCardSelector selectedCard = GetSelectedCard();

        if (selectedCard == null || selectedPlacement == null)
            return;

        GameObject mercenary = selectedCard.GetComponent<Card.PlayCard>().GetMercenary();
        mercenary.SetActive(true);
        mercenary.transform.position = selectedPlacement.transform.position;

        selectedPlacement.PlaceMerc(mercenary);
        GameObject handPlaceholder = selectedCard.GetComponent<Card.DragCard>().GetHandReplacement();
        hand.GetComponent<Card.HandPlacement>().ActivatePlaceholder(handPlaceholder, selectedCard.transform.GetSiblingIndex());
        cards.Remove(selectedCard);
        Destroy(selectedCard.gameObject);
        selectedPlacement = null;
        selectedCard = null;
    }

    private void FindPlacements()
    {
        GameObject[] placementAreas = GameObject.FindGameObjectsWithTag(Constants.PlacementAreaTag);

        for (int i = 0; i < placementAreas.Length; i++)
        {
            for (int j = 0; j < placementAreas[i].transform.childCount; j++)
            {
                GameObject area = placementAreas[i].transform.GetChild(j).gameObject;
                placements.Add(area.GetComponent<VRPlacementSelector>());
            }
        }
    }

    private void FindCards()
    {
        GameObject deck = GameObject.FindGameObjectWithTag(Constants.DeckTag);

        for (int i = 0; i < deck.transform.childCount; i++)
        {
            GameObject card = deck.transform.GetChild(i).gameObject;
            cards.Add(card.GetComponent<Card.VRCardSelector>());
        }
    }

    public VRPlacementSelector GetSelectedPlacement()
    {
        for (int i = 0; i < placements.Count; i++)
        {
            if (placements[i].IsSelected())
                return placements[i];
        }

        return null;
    }

    public Card.VRCardSelector GetSelectedCard()
    {
        for (int j = 0; j < cards.Count; j++)
        {
            if (cards[j].IsSelected())
                return cards[j];
        }

        return null;
    }
    
}
