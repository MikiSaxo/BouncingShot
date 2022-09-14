using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ReboundAnimation : MonoBehaviour
{
    [SerializeField] float durationMin, durationMax, durationEnd;
    [SerializeField] Vector3 minScale, maxScale;

    public void StartBounce()
    {
        transform.DOScale(minScale, durationMin).OnComplete(MaxBounce);
    }

    void MaxBounce()
    {
        transform.DOScale(maxScale, durationMax).OnComplete(IdleBounce);
    }

    void IdleBounce()
    {
        transform.DOScale(new Vector3(1, 1, 1), durationEnd);
    }
}
