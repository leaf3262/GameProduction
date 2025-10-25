using UnityEngine;
using UnityEngine.UI;

// MonoBehaviour for a single card GameObject. Displays data and handles clicks.
public class Card : MonoBehaviour
{
    public CardData data;  // Reference to the card's data (assigned in Inspector or code)
    public Image cardImage;  // UI Image to show the card sprite
    public TMPro.TextMeshProUGUI cardText;  // TMP text for rank/suit if no sprite

    private void Start()
    {
        UpdateDisplay();  // Show the card when it loads
    }

    // Update the card's visual based on data
    public void UpdateDisplay()
    {
        if (data != null)
        {
            if (cardImage != null && data.cardSprite != null)
            {
                cardImage.sprite = data.cardSprite;  // Show sprite if available
            }
            else if (cardText != null)
            {
                cardText.text = $"{data.rank} of {data.suit}";  // Fallback text
            }
        }
    }

    // Called when the card is clicked (we'll set this up in CardSlot)
    public void OnCardClicked()
    {
        // For now, just log (we'll handle discard in CardManager)
        Debug.Log($"Clicked {data.rank} of {data.suit}");
    }
}