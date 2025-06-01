using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip cardDealClip;

    [SerializeField] private AudioSource audioSource;
    
    public void PlayCardDealClip()
    {
        audioSource.PlayOneShot(cardDealClip);
    }
}
