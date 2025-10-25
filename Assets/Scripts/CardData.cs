using UnityEngine;

// ScriptableObject for card data (e.g., suit, rank, value). Create assets from this in Unity.
[CreateAssetMenu(fileName = "NewCard", menuName = "Card Legends/Card Data", order = 1)]
public class CardData : ScriptableObject
{
    public enum Suit { Hearts, Diamonds, Clubs, Spades }  // Card suits
    public enum Rank { Ace, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King }  // Card ranks

    [Header("Card Properties")]
    public Suit suit;  // The suit (e.g., Hearts)
    public Rank rank;  // The rank (e.g., Ace)
    public int value;  // Numerical value for scoring (e.g., Ace = 1 or 11, but we'll handle in logic later)
    public Sprite cardSprite;  // Image for the card (assign in Inspector)

    // Optional: For Power Cards later (e.g., special effects)
    [Header("Power Card (Optional)")]
    public bool isPowerCard = false;  // If true, this is a special card
    public string powerEffect = "";  // Description of effect (e.g., "Draw 2 extra cards")
}