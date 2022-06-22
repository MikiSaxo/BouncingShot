using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody2D rb;
    public float Speed;
    public float timer;
    public GameObject Child;
   
    void Start()
    {
        rb.velocity = transform.right * Speed * (timer * Speed / 2);
        Child.tag = tag;
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
        if (collision.transform.CompareTag("Outside") || collision.transform.CompareTag("Bumper") || collision.transform.CompareTag("P1") || collision.transform.CompareTag("P2"))
        {
            Destroy(gameObject);
        }

        if (collision.transform.CompareTag("Ball"))
        {
            GetComponent<CapsuleCollider2D>().isTrigger = false;
            //Destroy(gameObject);
        }

        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        print("ça touche manouch");
        if (collision.transform.CompareTag("BulletP1") && tag == "BulletP2")
        {
            print("P2 rencontre P1");
            if (collision.gameObject.GetComponent<Bullet>().timer > timer)
            {
                print("P2 > P1");
                Destroy(gameObject);
            }
            else if (collision.gameObject.GetComponent<Bullet>().timer == timer)
            {
                Destroy(collision.gameObject);
                Destroy(gameObject);
            }
        }

        if (collision.transform.CompareTag("BulletP2") && tag == "BulletP1")
        {
            print("P1 rencontre P2");
            if (collision.gameObject.GetComponent<Bullet>().timer > timer)
            {
                print("P1 > P2");
                Destroy(gameObject);
            }
            else if (collision.gameObject.GetComponent<Bullet>().timer == timer)
            {
                Destroy(collision.gameObject);
                Destroy(gameObject);
            }
        }
    }
}
