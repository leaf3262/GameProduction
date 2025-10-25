using UnityEngine;

[CreateAssetMenu(fileName = "NewCard", menuName = "Card Legends/Card Data", order = 1)]
public class CardData : ScriptableObject
{
    public enum Suit { Hearts, Diamonds, Clubs, Spades }
    public enum Rank { Ace, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King }

    [Header("Card Properties")]
    public Suit suit;
    public Rank rank;
    public int value;
    public Sprite cardSprite;

    [Header("Power Card (Optional)")]
    public bool isPowerCard = false;
    public enum PowerEffect { None, DrawExtra, MultiplyScore, DiscardExtra }
    public PowerEffect powerEffect;
    public int effectValue = 1;
}
