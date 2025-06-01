using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class CardManager : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform initialCardPos;
    public List<PlayerCardPos> allPlayersPos = new List<PlayerCardPos>();
    
    [SerializeField] private List<Transform> allGeneratedCards = new List<Transform>();
    private WaitForSeconds _waitTime;

    private List<int> _allAvailableCards;

    private void Awake()
    {
        _allAvailableCards = new List<int>();
        
        _waitTime = new WaitForSeconds(0.25f);
    }

    private void ResetAllAvailableCards()
    {
        _allAvailableCards = new List<int>();
        
        for (int i = 0; i < ReferenceManager.Instane.allCardImages.Count; i++)
        {
            _allAvailableCards.Add(i);
        }
    }

    [ContextMenu("DealCards")]
    public void DealCards()
    {
        StartCoroutine(Deal());
    }

    private IEnumerator Deal()
    {
        ResetAllAvailableCards();
        
        int startIndex = ReferenceManager.Instane.turnManager.TurnIndex;
        
        if(startIndex >= allPlayersPos.Count) yield break;
        
        for (int i = 0; i < ReferenceManager.Instane.turnManager.PlayerCount; i++)
        {
            int index = (i + startIndex) % ReferenceManager.Instane.turnManager.PlayerCount;
            for (int j = 0; j < 3; j++)
            {
                Transform card;
                if (allGeneratedCards.Count <= i*3 + j)
                {
                    card = Instantiate(cardPrefab, initialCardPos).transform;
                    allGeneratedCards.Add(card);
                }
                else
                {
                    card = allGeneratedCards[i*3 + j];
                }

                SetACard();
                PlayCardDealSound();
                allPlayersPos[index].AnimateInCard(card, j);
                
                yield return _waitTime;
            }
            yield return _waitTime;
        }
        
        ReferenceManager.Instane.dealerController.TriggerIdle();
    }

    private void PlayCardDealSound()
    {
        ReferenceManager.Instane.soundManager.PlayCardDealClip();
    }

    private void SetACard()
    {
        int cardIndex = Random.Range(0, _allAvailableCards.Count);

        ReferenceManager.Instane.uiManager.SetCard(_allAvailableCards[cardIndex]);
    }

    [ContextMenu("HideAllCards")]
    public void HideAllCards()
    {
        foreach (Transform card in allGeneratedCards)
        {
            card.gameObject.SetActive(false);
            card.transform.parent = initialCardPos;
            card.SetLocalPositionAndRotation(Vector3.zero, quaternion.identity);
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}