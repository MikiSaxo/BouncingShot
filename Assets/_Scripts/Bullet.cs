using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody2D rb;
    public float Speed;
    public float timer;
    public GameObject Child;
    public int bulletPower;

    void Start()
    {
        rb.velocity = transform.right * Speed;// * (timer * Speed / 2);
        //Child.tag = tag;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.Outside
            || collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.Bumper 
            || collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.P1
            || collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.P2)
        {
            Destroy(gameObject);
        }
        if (collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.BulletP1
            || collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.BulletP2)// || collision.transform.CompareTag("Ball"))
        {
            Destroy(gameObject);
            //GetComponent<CapsuleCollider2D>().isTrigger = true;
            //print("collision bullets");
        }
    }

 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.Outside
            || collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.Bumper
            || collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.P1
            || collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.P2)
        {
            Destroy(gameObject);
        }

        //if (collision.transform.CompareTag("Ball"))
        //{
            //GetComponent<CapsuleCollider2D>().isTrigger = false;
            //var test = transform.rotation.z;
            //print("test = " + test);
            //collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, test) * bulletPower, ForceMode2D.Impulse);
            //Destroy(gameObject);
        //}
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //print("ça touche manouch");
        /*if (collision.transform.CompareTag("BulletP1") && tag == "BulletP2")
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
        }*/
    }
}
