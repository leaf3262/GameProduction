using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NetworkedPlayer : NetworkBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Transform handArea;
    [SerializeField] private GameObject cardPrefab;

    public NetworkVariable<string> PlayerName = new(
        "",
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);

    private readonly List<GameObject> handCards = new();

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            string username = PlayerPrefs.GetString("Username", $"Player{OwnerClientId}");
            SetPlayerNameServerRpc(username);
        }

        PlayerName.OnValueChanged += OnNameChanged;
        OnNameChanged("", PlayerName.Value);
    }

    private void OnNameChanged(string oldName, string newName)
    {
        if (nameText != null)
            nameText.text = newName;
    }

    [ServerRpc]
    private void SetPlayerNameServerRpc(string newName)
    {
        PlayerName.Value = newName;
    }

    public void ClearHand()
    {
        foreach (var card in handCards)
            Destroy(card);
        handCards.Clear();
    }

    public void AddCard(int cardValue)
    {
        if (cardPrefab == null || handArea == null)
        {
            Debug.LogError("cardPrefab or handArea not assigned in inspector!");
            return;
        }

        GameObject cardGO = Instantiate(cardPrefab, handArea);
        handCards.Add(cardGO);

        var cardText = cardGO.GetComponentInChildren<TextMeshProUGUI>();
        if (cardText != null)
            cardText.text = cardValue.ToString();

        var button = cardGO.GetComponent<Button>();
        if (button != null)
        {
            int valueCopy = cardValue;
            button.onClick.AddListener(() => OnPlayCardButton(valueCopy));
        }
    }

    public void OnPlayCardButton(int cardValue)
    {
        if (IsOwner)
        {
            SubmitPlayServerRpc(cardValue);
            ClearHand();
        }
    }

    [ServerRpc]
    private void SubmitPlayServerRpc(int cardValue, ServerRpcParams rpcParams = default)
    {
        if (GameManager.Instance != null)
            GameManager.Instance.RegisterPlay(OwnerClientId, cardValue);
    }
}
