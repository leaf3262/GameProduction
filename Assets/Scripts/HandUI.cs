using System.Collections.Generic;
using UnityEngine;

// MonoBehaviour to manage the hand UI (5 slots).
public class HandUI : MonoBehaviour
{
    public List<CardSlot> cardSlots = new List<CardSlot>();  // 5 slots (assign in Inspector)

    // Update the UI with the current hand
    public void UpdateHand(List<CardData> hand)
    {
        for (int i = 0; i < cardSlots.Count; i++)
        {
            if (i < hand.Count)
            {
                cardSlots[i].SetCard(hand[i]);  // Assign card to slot
            }
            else
            {
                // Clear empty slots (if hand < 5)
                cardSlots[i].SetCard(null);
            }
        }
    }
}