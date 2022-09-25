using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BallDottedAnim : MonoBehaviour
{
    [SerializeField] private float _timeToReduce;
    void Start()
    {
        ScaleReduce();
    }

    private void ScaleReduce()
    {
        transform.DOScale(Vector2.one * .2f, _timeToReduce).OnComplete(ScaleGrow);
    }

    private void ScaleGrow()
    {
        transform.DOScale(Vector2.one, 0).OnComplete(ScaleReduce);
    }
}
