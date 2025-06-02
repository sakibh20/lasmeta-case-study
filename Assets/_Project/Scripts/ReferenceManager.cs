using System.Collections.Generic;
using UnityEngine;

public class ReferenceManager : MonoBehaviour
{
    public TurnManager turnManager;
    public UiManager uiManager;
    public DealerController dealerController;
    public DealerAnimationManager dealerAnimationManager;
    public CardManager cardManager;
    public SoundManager soundManager;
    public PlayerSpawner playerSpawner;
    public PlayerMovement playerMovement;
    public List<Sprite> allCardImages = new List<Sprite>();
    
    public Transform markerPos1;
    public Transform markerPos2;
    
    public static ReferenceManager Instane;

    private void Awake()
    {
        if (Instane == null)
        {
            Instane = this;
            return;
        }
        Destroy(gameObject);
    }
}
