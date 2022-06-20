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
        if (collision.transform.CompareTag("Outside") || collision.transform.CompareTag("Ball") || collision.transform.CompareTag("Bumper") || collision.transform.CompareTag("P1") || collision.transform.CompareTag("P2") || collision.transform.CompareTag("Bullet"))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Outside"))
        {
            Destroy(gameObject);
        }
    }

}
