using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerCardPos : MonoBehaviour
{
    public List<Transform> cardPos = new List<Transform>();

    private Transform _card;
    private readonly float _animDuration = 0.3f;
    public void AnimateInCard(Transform card, int index)
    {
        if(index >= cardPos.Count) return;
        
        _card = card;

        _card.parent = cardPos[index];
        DoInAnimation();
    }

    private void DoInAnimation()
    {
        _card.gameObject.SetActive(true);
        _card.DOLocalMove(Vector3.zero, _animDuration);
        _card.DOLocalRotate(Vector3.zero, _animDuration);
    }
}
