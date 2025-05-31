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
        DealerAnimationManager.Instance.PlayDealAnimation();
    }
    
    [Rpc(RpcSources.All, RpcTargets.All)]
    private void Rpc_Idle()
    {
        DealerAnimationManager.Instance.PlayIdleAnimation();
    }
}