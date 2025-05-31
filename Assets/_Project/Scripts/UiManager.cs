using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] private DealerController dealer;
    [SerializeField] private TurnManager turnManager;

    [SerializeField] private Button startButton;
    [SerializeField] private Button nextRoundButton;
    [SerializeField] private Button dealButton;

    [SerializeField] private TextMeshProUGUI messageText;
    
    public static UiManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            return;
        }
        
        Destroy(gameObject);
    }

    private void Start()
    {
        ShowMessage();
        DisableInteraction();
    }

    public void OnStartGamePressed()
    {
        if (turnManager.Object.HasStateAuthority)
        {
            turnManager.StartGame();
        }
    }
    
    public void OnNextTurnPressed()
    {
        if (turnManager.Object.HasStateAuthority)
        {
            turnManager.NextTurn();
        }
    }    
    
    public void OnDealButtonPressed()
    {
        if (dealer != null)
        {
            dealer.TriggerDeal();
        }
    }
    
    public void OnIdleButtonPressed()
    {
        if (dealer != null)
        {
            dealer.TriggerIdle();
        }
    }

    public void ShowMessage()
    {
        messageText.text = "Waiting for others";
    }

    public void DisableInteraction()
    {
        startButton.interactable = false;
        nextRoundButton.interactable = false;
        dealButton.interactable = false;
    }
    
    public void EnableInteraction()
    {
        startButton.interactable = true;
        nextRoundButton.interactable = true;
        dealButton.interactable = true;
    }
}
