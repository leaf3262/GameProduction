using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [Header("Deck Settings")]
    public List<CardData> fullDeck = new List<CardData>();
    private List<CardData> currentDeck = new List<CardData>();
    private List<CardData> hand = new List<CardData>();

    [Header("References")]
    public HandUI handUI;

    private void Start()
    {
        InitializeDeck();
        DrawInitialHand();
    }

    public void InitializeDeck()
    {
        currentDeck = new List<CardData>(fullDeck);
        ShuffleDeck();
    }

    private void ShuffleDeck()
    {
        for (int i = currentDeck.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            CardData temp = currentDeck[i];
            currentDeck[i] = currentDeck[randomIndex];
            currentDeck[randomIndex] = temp;
        }
    }

    public void DrawInitialHand()
    {
        for (int i = 0; i < 5; i++)
        {
            DrawCard();
        }
        handUI.UpdateHand(hand);
    }

    public void DrawCard()
    {
        if (currentDeck.Count > 0)
        {
            CardData drawn = currentDeck[0];
            currentDeck.RemoveAt(0);
            hand.Add(drawn);
        }
    }

    public void DiscardAndDraw(CardData toDiscard)
    {
        if (hand.Contains(toDiscard))
        {
            hand.Remove(toDiscard);
            DrawCard();
            handUI.UpdateHand(hand);
        }
    }

    public List<CardData> GetHand()
    {
        return new List<CardData>(hand);
    }
}
