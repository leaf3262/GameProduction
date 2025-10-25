using UnityEngine;
using UnityEngine.UI;

// MonoBehaviour for a card slot in the hand UI. Holds a Card and handles clicks.
public class CardSlot : MonoBehaviour
{
    public Card card;  // The card in this slot
    public Button slotButton;  // Button to click the card
    public GameObject cardPrefab;  // Assign CardPrefab here in Inspector (instead of Resources.Load)

    private void Start()
    {
        if (slotButton != null)
        {
            slotButton.onClick.AddListener(OnSlotClicked);  // Listen for clicks
        }
    }

    // Assign a card to this slot (or clear if null)
    public void SetCard(CardData data)
    {
        if (data != null)
        {
            if (card == null)
            {
                // Instantiate the card prefab (assigned in Inspector)
                if (cardPrefab != null)
                {
                    GameObject cardObj = Instantiate(cardPrefab);
                    cardObj.transform.SetParent(transform, false);  // Parent to this slot
                    card = cardObj.GetComponent<Card>();
                }
                else
                {
                    Debug.LogError("CardPrefab not assigned in CardSlot Inspector! Drag CardPrefab into the field.");
                    return;
                }
            }
            card.data = data;
            card.UpdateDisplay();
        }
        else
        {
            // Clear the slot: Destroy the card GameObject if it exists
            if (card != null)
            {
                Destroy(card.gameObject);
                card = null;
            }
        }
    }

    // Handle click: Discard and draw new (calls CardManager)
    private void OnSlotClicked()
    {
        if (card != null && card.data != null)
        {
            FindObjectOfType<CardManager>().DiscardAndDraw(card.data);  // Tell manager to handle
        }
    }
}