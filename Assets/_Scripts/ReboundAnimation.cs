using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ReboundAnimation : MonoBehaviour
{
    [SerializeField] float durationMin, durationMax, durationEnd, minScale, maxScale;
    void Start()
    {
        //StartBounce();
    }

    public void StartBounce()
    {
        transform.DOScale(new Vector3(minScale, minScale, minScale), durationMin).OnComplete(MaxBounce);
    }

    void MaxBounce()
    {
        transform.DOScale(new Vector3(maxScale, maxScale, maxScale), durationMax).OnComplete(IdleBounce);
    }

    void IdleBounce()
    {
        transform.DOScale(new Vector3(1, 1, 1), durationEnd);
    }
}
