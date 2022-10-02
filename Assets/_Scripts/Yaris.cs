using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Yaris : MonoBehaviour
{
    [SerializeField] private float speed = 0f;
    [SerializeField] private Rigidbody2D rbBall;
    //void Start()
    //{
    //    transform.DORotate(new Vector3(0, 0, 360), speed, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
    //}

    private void Update()
    {
        var maxVelo = Mathf.Max(rbBall.velocity.x, rbBall.velocity.y);
        transform.Rotate(new Vector3(0, 0, 1 * maxVelo));
    }
}
