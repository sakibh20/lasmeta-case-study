using Fusion;
using UnityEngine;

public class TurnManager : NetworkBehaviour, IStateAuthorityChanged
{
    [SerializeField] private int minRequiredPlayers = 2;
    private NetworkObject _dealerNetworkObject;
    [Networked] public int TurnIndex { get; set; }
    
    [Networked, Capacity(10)]
    private NetworkLinkedList<PlayerRef> SyncedPlayerList => default;

    public int PlayerCount => SyncedPlayerList.Count;
    
    private bool _initialized;

    private void Awake()
    {
        _dealerNetworkObject = GetComponent<NetworkObject>();
    }

    public override void Spawned()
    {
        if (!HasStateAuthority)
        {
            ReferenceManager.Instane.uiManager.DisableInteraction();
        }
        
        if (!_initialized)
        {
            _initialized = true;
        }
    }
    
    public void PlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if(!_initialized) return;
        if(!IsValid()) return;
        if (SyncedPlayerList.Contains(player)) return;
        
        SyncedPlayerList.Add(player);
        ReferenceManager.Instane.uiManager.ShowMessage("Waiting for others");
        if (SyncedPlayerList.Count < minRequiredPlayers) return;

        InitiateGame();
    }

    private void InitiateGame()
    {
        ReferenceManager.Instane.uiManager.ShowMessage("Starting Game...", false, 3);
        
        Invoke(nameof(StartGame), 3);
    }

    public void PlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if(!_initialized) return;
        if(!IsValid()) return;
        if (!SyncedPlayerList.Contains(player)) return;
        
        SyncedPlayerList.Remove(player);
        
        if (SyncedPlayerList.Count >= minRequiredPlayers) return;
        ReferenceManager.Instane.uiManager.ShowMessage("Waiting for others");
        ReferenceManager.Instane.uiManager.DisableInteraction();
    }
    
    private bool IsValid()
    {
        if (Runner == null || Object == null || !Object.HasStateAuthority) return false;
        return true;
    }

    public void StartGame()
    {
        // if (SyncedPlayerList.Count == 0)
        // {
        //     Debug.LogWarning("Cannot start game: No players in PlayerManager");
        //     return;
        // }
        //
        // TurnIndex = 0;
        // RPC_GiveAuthorityToCurrentPlayer();
        ReferenceManager.Instane.uiManager.EnableInteraction();
    }

    public void NextTurn()
    {
        if (SyncedPlayerList.Count == 0)
        {
            Debug.LogWarning("No players in list");
            return;
        }
        
        if (!HasStateAuthority) return;
        
        ReleaseAuthority();
        
        TurnIndex = (TurnIndex + 1) % SyncedPlayerList.Count;
        
        RPC_GiveAuthorityToCurrentPlayer();
    }

    private void ReleaseAuthority()
    {
        Debug.Log($"Releasing authority");
        _dealerNetworkObject.ReleaseStateAuthority();
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    private void RPC_GiveAuthorityToCurrentPlayer()
    {
        PlayerRef currentPlayer = SyncedPlayerList[TurnIndex];
        Debug.Log($"RequestStateAuthority: {currentPlayer}");
        
        ReferenceManager.Instane.cardManager.HideAllCards();
        
        if (Runner.LocalPlayer == currentPlayer)
        {
            _dealerNetworkObject.RequestStateAuthority();
        }
    }

    public void StateAuthorityChanged()
    {
        PlayerRef currentPlayer = SyncedPlayerList[TurnIndex];
        
        if (Runner.LocalPlayer == currentPlayer)
        {
            ReferenceManager.Instane.uiManager.EnableInteraction();
        }
        else
        {
            ReferenceManager.Instane.uiManager.DisableInteraction();
        }
    }
}