using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody2D rb;
    public float Speed;
    public float timer;
    void Start()
    {
        rb.velocity = transform.right * Speed * (timer * Speed / 2);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Outside") || collision.transform.CompareTag("Bumper"))// || collision.transform.CompareTag("P1") || collision.transform.CompareTag("P2"))
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
