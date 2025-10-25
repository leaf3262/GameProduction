using System.Collections.Generic;
using UnityEngine;

public class HandUI : MonoBehaviour
{
    public List<CardSlot> cardSlots = new List<CardSlot>();

    public void UpdateHand(List<CardData> hand)
    {
        for (int i = 0; i < cardSlots.Count; i++)
        {
            if (i < hand.Count)
            {
                cardSlots[i].SetCard(hand[i]);
            }
            else
            {
                cardSlots[i].SetCard(null);
            }
        }
    }
}
