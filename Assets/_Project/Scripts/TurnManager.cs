using Fusion;
using UnityEngine;

public class TurnManager : NetworkBehaviour, IStateAuthorityChanged
{
    [SerializeField] private NetworkObject dealerNetworkObject;
    [Networked] public int TurnIndex { get; set; }
    
    [Networked, Capacity(10)]
    private NetworkLinkedList<PlayerRef> SyncedPlayerList => default;
    
    private bool _initialized;
    
    public override void Spawned()
    {
        if (!HasStateAuthority)
        {
            UiManager.Instance.DisableInteraction();
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

        if (SyncedPlayerList.Count < 3) return;

        UiManager.Instance.EnableInteraction();
        //StartGame();
    }

    public void PlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if(!_initialized) return;
        if(!IsValid()) return;
        if (!SyncedPlayerList.Contains(player)) return;
        
        SyncedPlayerList.Remove(player);
    }
    
    private bool IsValid()
    {
        if (Runner == null || Object == null || !Object.HasStateAuthority) return false;
        return true;
    }

    public void StartGame()
    {
        if (SyncedPlayerList.Count == 0)
        {
            Debug.LogWarning("Cannot start game: No players in PlayerManager");
            return;
        }

        TurnIndex = 0;
        RPC_GiveAuthorityToCurrentPlayer();
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
        dealerNetworkObject.ReleaseStateAuthority();
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    private void RPC_GiveAuthorityToCurrentPlayer()
    {
        PlayerRef currentPlayer = SyncedPlayerList[TurnIndex];
        Debug.Log($"RequestStateAuthority: {currentPlayer}");
        
        if (Runner.LocalPlayer == currentPlayer)
        {
            dealerNetworkObject.RequestStateAuthority();
        }
    }

    public void StateAuthorityChanged()
    {
        PlayerRef currentPlayer = SyncedPlayerList[TurnIndex];
        
        if (Runner.LocalPlayer == currentPlayer)
        {
            UiManager.Instance.EnableInteraction();
        }
        else
        {
            UiManager.Instance.DisableInteraction();
        }
    }
}