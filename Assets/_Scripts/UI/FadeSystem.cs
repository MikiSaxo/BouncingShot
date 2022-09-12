using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FadeSystem : MonoBehaviour
{
    [SerializeField] GameObject[] sideFade = null;
    float[] sideFadeStartPosX = new float[2];

    private void Start()
    {
        sideFadeStartPosX[0] = sideFade[0].transform.position.x;
        sideFadeStartPosX[1] = sideFade[1].transform.position.x;
        print(sideFadeStartPosX[0]);
        FadeOn();
    }

    void FadeOn()
    {
        sideFade[0].transform.DOMoveX(0, 1f);
        sideFade[1].transform.DOMoveX(0, 1f).OnComplete(FadeOff);
    }

    void FadeOff()
    {
        sideFade[0].transform.DOMoveX(sideFadeStartPosX[0], 1f);
        sideFade[1].transform.DOMoveX(sideFadeStartPosX[1], 1f);
    }
}
