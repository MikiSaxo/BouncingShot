using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody2D rb;
    public float Speed;
    void Start()
    {
        rb.velocity = transform.right * Speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Outside") || collision.transform.CompareTag("Ball"))
        {
            Destroy(gameObject);
        }
    }
}
