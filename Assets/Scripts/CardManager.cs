using System.Collections.Generic;
using UnityEngine;

// MonoBehaviour to manage the deck, hand, and card actions.
public class CardManager : MonoBehaviour
{
    [Header("Deck Settings")]
    public List<CardData> fullDeck = new List<CardData>();  // All possible cards (assign in Inspector)
    private List<CardData> currentDeck = new List<CardData>();  // Shuffled deck
    private List<CardData> hand = new List<CardData>();  // Player's 5-card hand

    [Header("References")]
    public HandUI handUI;  // UI to display the hand

    private void Start()
    {
        InitializeDeck();
        DrawInitialHand();
    }

    // Load and shuffle the deck
    private void InitializeDeck()
    {
        currentDeck = new List<CardData>(fullDeck);  // Copy the full deck
        ShuffleDeck();
    }

    // Simple shuffle (Fisher-Yates)
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

    // Draw 5 cards for initial hand
    private void DrawInitialHand()
    {
        for (int i = 0; i < 5; i++)
        {
            DrawCard();
        }
        handUI.UpdateHand(hand);  // Tell UI to show the hand
    }

    // Draw one card from deck to hand
    private void DrawCard()
    {
        if (currentDeck.Count > 0)
        {
            CardData drawn = currentDeck[0];
            currentDeck.RemoveAt(0);
            hand.Add(drawn);
        }
    }

    // Discard a card and draw a new one
    public void DiscardAndDraw(CardData toDiscard)
    {
        if (hand.Contains(toDiscard))
        {
            hand.Remove(toDiscard);
            DrawCard();
            handUI.UpdateHand(hand);  // Refresh UI
        }
    }
}