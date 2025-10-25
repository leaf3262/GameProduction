using UnityEngine;
using UnityEngine.UI;

public class CardSlot : MonoBehaviour
{
    public Card card;
    public Button slotButton;
    public GameObject cardPrefab;

    private void Start()
    {
        if (slotButton != null)
        {
            slotButton.onClick.AddListener(OnSlotClicked);
        }
    }

    public void SetCard(CardData data)
    {
        if (data != null)
        {
            if (card == null)
            {
                if (cardPrefab != null)
                {
                    GameObject cardObj = Instantiate(cardPrefab);
                    cardObj.transform.SetParent(transform, false);
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
            if (card != null)
            {
                Destroy(card.gameObject);
                card = null;
            }
        }
    }

    private void OnSlotClicked()
    {
        if (card != null && card.data != null)
        {
            FindObjectOfType<CardManager>().DiscardAndDraw(card.data);
        }
    }
}
