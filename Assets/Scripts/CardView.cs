using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private Button playButton;

    private int cardValue;
    private NetworkedPlayer owner;

    public void SetCardValue(int value)
    {
        cardValue = value;
        if (valueText != null) valueText.text = value.ToString();
    }

    public void SetOwner(NetworkedPlayer player)
    {
        owner = player;
        if (playButton != null)
        {
            playButton.onClick.RemoveAllListeners();
            playButton.onClick.AddListener(() => owner.OnPlayCardButton(cardValue));
        }
    }
}
