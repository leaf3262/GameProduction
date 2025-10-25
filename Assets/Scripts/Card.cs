using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public CardData data;
    public Image cardImage;
    public TMPro.TextMeshProUGUI cardText;

    private void Start()
    {
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        if (data != null)
        {
            if (cardImage != null && data.cardSprite != null)
            {
                cardImage.sprite = data.cardSprite;
            }
            else if (cardText != null)
            {
                cardText.text = $"{data.rank} of {data.suit}";
            }
        }
    }

    public void OnCardClicked()
    {
        Debug.Log($"Clicked {data.rank} of {data.suit}");
    }
}
