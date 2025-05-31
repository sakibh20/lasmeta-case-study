using UnityEngine;

public class DealerAnimationManager : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Deal = Animator.StringToHash("Deal");

    public static DealerAnimationManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            return;
        }
        
        Destroy(gameObject);
    }

    public void PlayDealAnimation()
    {
        //Debug.Log("TriggerDeal");
        animator.SetBool(Deal, true);
        animator.SetBool(Idle, false);
    }
    
    public void PlayIdleAnimation()
    {
        //Debug.Log("TriggerIdle");
        animator.SetBool(Deal, false);
        animator.SetBool(Idle, true);
    }
}
