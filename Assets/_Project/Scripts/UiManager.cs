using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI cameraViewInstructionText;
    [SerializeField] private Button nextRoundButton;
    [SerializeField] private Button dealButton;
    [SerializeField] private Toggle viewToggle;

    [SerializeField] private RectTransform messagePanel;
    [SerializeField] private TextMeshProUGUI messageText;

    [SerializeField] private List<Image> cardPlaceholders = new List<Image>();

    private ReferenceManager _referenceManager;

    private int _currentCardIndex;
    private Vector2 _messagePanelInitialPos;

    private string thirdPersonViewText = "Scroll to Zoom & Drag to change camera angle";
    private string firstPersonViewText = "Use Arrow Keys or WASD to move";

    private void Awake()
    {
        _messagePanelInitialPos = messagePanel.anchoredPosition;
        HideMessagePanel();
    }

    private void Start()
    {
        _referenceManager = ReferenceManager.Instane;
        viewToggle.onValueChanged.AddListener(OnViewValueChanged);
        DisableInteraction();
    }

    private void OnViewValueChanged(bool isOn)
    {
        if (_referenceManager == null) return;
        if (_referenceManager.playerMovement == null) return;

        if (isOn)
        {
            UpdateViewInstruction(firstPersonViewText);
            _referenceManager.playerMovement.BackToFirstPersonView();
            return;
        }

        UpdateViewInstruction(thirdPersonViewText);
        HideMessagePanel();
        _referenceManager.playerMovement.BackToThirdPersonView();
    }

    public void OnNextTurnPressed()
    {
        if (_referenceManager.turnManager.Object.HasStateAuthority)
        {
            CancelInvoke(nameof(ShowNextMessage));
            HideMessagePanel();
            _referenceManager.turnManager.NextTurn();
        }
    }

    public void OnDealButtonPressed()
    {
        if (_referenceManager.turnManager != null)
        {
            dealButton.interactable = false;
            HideMessagePanel();
            Invoke(nameof(ShowNextMessage), 2.0f);
            _referenceManager.dealerController.TriggerDeal();
        }
    }

    private void ShowNextMessage()
    {
        ShowMessage("Click Next Round button to end this round");
    }

    public void ShowMessage(string message, bool stayVisible = true, float visibleDuration = 0)
    {
        if (string.IsNullOrWhiteSpace(message)) return;
        if (message == messageText.text) return;

        ShowMessagePanel(message);

        if (!stayVisible)
        {
            Invoke(nameof(HideMessagePanelNoCallback), visibleDuration);
        }
    }

    private void ShowMessagePanel(string message)
    {
        messageText.text = message;
        HideMessagePanel(() =>
        {
            messagePanel.DOAnchorPos(_messagePanelInitialPos, 0.2f).SetEase(Ease.InBounce);
        });
    }

    private void HideMessagePanel(Action onComplete = null)
    {
        messagePanel.DOAnchorPos(_messagePanelInitialPos + new Vector2(700, 0), 0.2f).SetEase(Ease.OutBounce).OnComplete(() =>
        {
            onComplete?.Invoke();
        });
    }
    private void HideMessagePanelNoCallback()
    {
        messagePanel.DOAnchorPos(_messagePanelInitialPos + new Vector2(700, 0), 0.2f).SetEase(Ease.OutBounce);
    }

    public void DisableInteraction()
    {
        nextRoundButton.interactable = false;
        dealButton.interactable = false;

        HideAllCards();
    }

    public void EnableInteraction()
    {
        nextRoundButton.interactable = true;
        dealButton.interactable = true;

        ShowMessage("Start deal when ready");
        HideAllCards();
    }

    public void SetCard(int drawnCardIndex)
    {
        if (_currentCardIndex >= cardPlaceholders.Count) return;
        if (drawnCardIndex >= ReferenceManager.Instane.allCardImages.Count) return;

        cardPlaceholders[_currentCardIndex].sprite = ReferenceManager.Instane.allCardImages[drawnCardIndex];
        cardPlaceholders[_currentCardIndex].enabled = true;

        _currentCardIndex += 1;
    }

    private void HideAllCards()
    {
        _currentCardIndex = 0;
        foreach (Image image in cardPlaceholders)
        {
            image.enabled = false;
        }
    }

    public void UpdateViewInstruction(string message)
    {
        cameraViewInstructionText.text = message;
    }

    public void EnableCameraViewToggle(bool value)
    {
        viewToggle.interactable = value;
    }
}
