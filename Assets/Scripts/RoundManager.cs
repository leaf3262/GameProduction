using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoundManager : MonoBehaviour
{
    [Header("References")]
    public CardManager cardManager;
    public TMP_Text scoreText;
    public TMP_Text resultText;

    private int currentScore = 0;
    private int targetScore = 100;

    private void Start()
    {
        StartNewRound();
    }

    public void StartNewRound()
    {
        cardManager.InitializeDeck();
        cardManager.DrawInitialHand();
        CalculateScore();
        UpdateUI();
    }

    private void CalculateScore()
    {
        List<CardData> hand = cardManager.GetHand();
        currentScore = 0;

        foreach (CardData card in hand)
        {
            currentScore += card.value;
        }

        if (IsPair(hand)) currentScore += 20;
        if (IsStraight(hand)) currentScore += 50;
        if (IsFlush(hand)) currentScore += 100;

        foreach (CardData card in hand)
        {
            if (card.isPowerCard)
            {
                switch (card.powerEffect)
                {
                    case CardData.PowerEffect.DrawExtra:
                        for (int i = 0; i < card.effectValue; i++)
                            cardManager.DrawCard();
                        break;

                    case CardData.PowerEffect.MultiplyScore:
                        currentScore *= card.effectValue;
                        break;
                }
            }
        }

        CheckWinLoss();
    }

    private void CheckWinLoss()
    {
        if (currentScore >= targetScore)
        {
            resultText.text = "You Win! Score: " + currentScore;
        }
        else
        {
            resultText.text = "Round Ongoing. Score: " + currentScore;
        }
    }

    private void UpdateUI()
    {
        scoreText.text = "Score: " + currentScore;
    }

    private bool IsPair(List<CardData> hand)
    {
        for (int i = 0; i < hand.Count; i++)
            for (int j = i + 1; j < hand.Count; j++)
                if (hand[i].rank == hand[j].rank) return true;
        return false;
    }

    private bool IsStraight(List<CardData> hand)
    {
        hand.Sort((a, b) => (int)a.rank - (int)b.rank);
        for (int i = 0; i < hand.Count - 1; i++)
            if ((int)hand[i + 1].rank != (int)hand[i].rank + 1) return false;
        return true;
    }

    private bool IsFlush(List<CardData> hand)
    {
        CardData.Suit suit = hand[0].suit;
        foreach (CardData card in hand)
            if (card.suit != suit) return false;
        return true;
    }
}
