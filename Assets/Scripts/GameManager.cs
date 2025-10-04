using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using TMPro;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private int maxRoundsToWin = 3;
    [SerializeField] private TextMeshProUGUI roundWinnerText;
    [SerializeField] private Transform playArea;

    private readonly List<ulong> playerClientIds = new List<ulong>();
    private int currentRound = 0;
    private readonly Dictionary<ulong, int> roundPlays = new Dictionary<ulong, int>();
    private readonly Dictionary<ulong, int> scores = new Dictionary<ulong, int>();

    private void Awake() { Instance = this; }

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;
        var players = Object.FindObjectsByType<NetworkedPlayer>(FindObjectsSortMode.None);
        foreach (var p in players) { playerClientIds.Add(p.OwnerClientId); scores[p.OwnerClientId] = 0; }
        StartCoroutine(StartMatch());
    }

    private IEnumerator StartMatch()
    {
        yield return new WaitForSeconds(1f);
        DealCards();
        StartNewRound();
    }

    private void DealCards()
    {
        foreach (var clientId in playerClientIds)
        {
            var player = FindPlayerByClientId(clientId);
            if (player == null) continue;
            player.ClearHand();
            for (int i = 0; i < 5; i++) player.AddCard(Random.Range(1, 14));
        }
    }

    private NetworkedPlayer FindPlayerByClientId(ulong clientId)
    {
        var players = Object.FindObjectsByType<NetworkedPlayer>(FindObjectsSortMode.None);
        foreach (var p in players) if (p.OwnerClientId == clientId) return p;
        return null;
    }

    private void StartNewRound()
    {
        currentRound++;
        roundPlays.Clear();
        if (roundWinnerText != null) roundWinnerText.text = $"Round {currentRound} started!";
    }

    public void RegisterPlay(ulong clientId, int cardValue)
    {
        if (!IsServer) return;
        if (!roundPlays.ContainsKey(clientId)) roundPlays[clientId] = cardValue;
        if (roundPlays.Count == playerClientIds.Count) ResolveRound();
    }

    private void ResolveRound()
    {
        ulong winnerId = 0; int highest = -1;
        foreach (var kv in roundPlays) if (kv.Value > highest) { highest = kv.Value; winnerId = kv.Key; }
        if (winnerId != 0)
        {
            scores[winnerId]++;
            DeclareRoundWinner(winnerId, highest);
            if (scores[winnerId] >= maxRoundsToWin) EndMatch(winnerId);
            else StartNewRound();
        }
    }

    private void DeclareRoundWinner(ulong winnerId, int cardValue)
    {
        var player = FindPlayerByClientId(winnerId);
        if (roundWinnerText != null && player != null) roundWinnerText.text = $"{player.PlayerName.Value} wins round {currentRound} with {cardValue}!";
    }

    private void EndMatch(ulong winnerId)
    {
        var player = FindPlayerByClientId(winnerId);
        if (roundWinnerText != null && player != null) roundWinnerText.text = $"{player.PlayerName.Value} wins the match!";
    }

    public void RequestDeal()
    {
        if (!IsServer) return;
        DealCards();
    }
}
