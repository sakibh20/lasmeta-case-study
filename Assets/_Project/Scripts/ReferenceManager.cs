using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReferenceManager : MonoBehaviour
{
    public TurnManager turnManager;
    public UiManager uiManager;
    public DealerController dealerController;
    public DealerAnimationManager dealerAnimationManager;
    public CardManager cardManager;
    public SoundManager soundManager;
    
    public List<Sprite> allCardImages = new List<Sprite>();
    
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
