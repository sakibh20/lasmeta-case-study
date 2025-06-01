using Fusion;

public class DealerController : NetworkBehaviour
{
    public void TriggerIdle()
    {
        if (Object.HasStateAuthority)
        {
            Rpc_Idle();
        }
    }

    public void TriggerDeal()
    {
        if (Object.HasStateAuthority)
        {
            Rpc_Dealing();
        }
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    private void Rpc_Dealing()
    {
        HandleDeal();
    }

    private void HandleDeal()
    {
        ReferenceManager.Instane.dealerAnimationManager.PlayDealAnimation();
        ReferenceManager.Instane.cardManager.DealCards();
    }
    
    [Rpc(RpcSources.All, RpcTargets.All)]
    private void Rpc_Idle()
    {
        ReferenceManager.Instane.dealerAnimationManager.PlayIdleAnimation();
    }
}