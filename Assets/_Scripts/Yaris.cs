using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Yaris : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rbBall;

    private void Update()
    {
        var maxVelo = Mathf.Max(rbBall.velocity.x, rbBall.velocity.y);
        transform.Rotate(new Vector3(0, 0, 1 * maxVelo));
    }
}
