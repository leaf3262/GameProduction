using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class NetworkLobbyManager : NetworkBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_InputField chatInput;
    [SerializeField] private RectTransform chatContent;
    [SerializeField] private TextMeshProUGUI chatMessagePrefab;
    [SerializeField] private TMP_InputField joinCodeInput;
    [SerializeField] private RectTransform playerListContent;
    [SerializeField] private TextMeshProUGUI playerListEntryPrefab;
    [SerializeField] private TextMeshProUGUI walletBalanceText;

    private readonly List<NetworkedPlayer> connectedPlayers = new List<NetworkedPlayer>();

    private void Start()
    {
        if (chatInput != null)
        {
            chatInput.onSubmit.AddListener(OnChatSubmit);
            chatInput.onEndEdit.AddListener(HandleEndEdit);
        }
    }

    private void OnDestroy()
    {
        if (chatInput != null)
        {
            chatInput.onSubmit.RemoveListener(OnChatSubmit);
            chatInput.onEndEdit.RemoveListener(HandleEndEdit);
        }
    }

    private void HandleEndEdit(string text)
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            Debug.Log("[Chat] EndEdit triggered, submitting message: " + text);
            OnChatSubmit(text);
            chatInput.text = string.Empty;
            chatInput.ActivateInputField();
        }
    }

    private void OnChatSubmit(string message)
    {
        if (string.IsNullOrWhiteSpace(message)) return;

        Debug.Log("[Chat] OnChatSubmit called with message: " + message);

        chatInput.text = string.Empty;
        chatInput.ActivateInputField();
        OnChatMessageSubmitted(message);
    }

    public void OnChatMessageSubmitted(string message)
    {
        if (string.IsNullOrWhiteSpace(message)) return;

        Debug.Log("[Chat] OnChatMessageSubmitted -> " + message);

        if (IsServer)
        {
            ulong localId = NetworkManager.Singleton != null ? NetworkManager.Singleton.LocalClientId : 0;
            Debug.Log("[Chat] Server broadcasting message: " + message + " from clientId " + localId);
            BroadcastChatMessageClientRpc(localId, message);
        }
        else
        {
            Debug.Log("[Chat] Client sending message to server: " + message);
            SendChatMessageServerRpc(message);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SendChatMessageServerRpc(string message, ServerRpcParams rpcParams = default)
    {
        ulong senderId = rpcParams.Receive.SenderClientId;
        Debug.Log("[Chat] Server received message from clientId " + senderId + ": " + message);
        BroadcastChatMessageClientRpc(senderId, message);
    }

    [ClientRpc]
    private void BroadcastChatMessageClientRpc(ulong senderId, string message)
    {
        Debug.Log("[Chat] BroadcastChatMessageClientRpc -> senderId " + senderId + " message: " + message);
        DisplayChatMessage(senderId, message);
    }

    private void DisplayChatMessage(ulong senderId, string message)
    {
        string playerName = $"Player {senderId}";
        foreach (var p in connectedPlayers)
        {
            if (p == null) continue;
            if (p.OwnerClientId == senderId)
            {
                if (!string.IsNullOrEmpty(p.PlayerName.Value))
                    playerName = p.PlayerName.Value;
                break;
            }
        }

        Debug.Log("[Chat] Displaying message: " + playerName + ": " + message);

        if (chatMessagePrefab == null || chatContent == null)
        {
            Debug.LogError("[Chat] ERROR: ChatMessagePrefab or ChatContent not assigned in Inspector!");
            return;
        }

        TextMeshProUGUI chatEntry = Instantiate(chatMessagePrefab, chatContent);
        if (chatEntry != null)
        {
            chatEntry.text = $"{playerName}: {message}";
            Debug.Log("[Chat] Chat entry instantiated and added to panel.");
        }
    }

    public void RegisterPlayer(NetworkedPlayer player)
    {
        if (!connectedPlayers.Contains(player))
        {
            connectedPlayers.Add(player);
            UpdatePlayerListUI();
        }
    }

    public void UnregisterPlayer(NetworkedPlayer player)
    {
        if (connectedPlayers.Contains(player))
        {
            connectedPlayers.Remove(player);
            UpdatePlayerListUI();
        }
    }

    private void UpdatePlayerListUI()
    {
        if (playerListContent == null || playerListEntryPrefab == null) return;

        foreach (Transform child in playerListContent)
        {
            Destroy(child.gameObject);
        }

        foreach (var player in connectedPlayers)
        {
            if (player == null) continue;
            TextMeshProUGUI entry = Instantiate(playerListEntryPrefab, playerListContent);
            entry.text = player.PlayerName.Value;
        }
    }

    public void CreateMatchClicked()
    {
        Debug.Log("CreateMatchClicked triggered");
    }

    public void JoinMatchClicked()
    {
        Debug.Log("JoinMatchClicked triggered with code: " + (joinCodeInput != null ? joinCodeInput.text : "<no code>"));
    }

    public void OnReadyClicked()
    {
        Debug.Log("OnReadyClicked triggered");
    }

    public void OnStartMatchClicked()
    {
        Debug.Log("OnStartMatchClicked triggered");
    }
}
