using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody2D rb;
    public float Speed;
    public float timer;
    public GameObject Child;
    public int bulletPower, bouncePlayerPower;

    void Start()
    {
        rb.velocity = transform.right * Speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.Outside //Outside & Goals & Bullets
            || collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.BulletP1
            || collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.BulletP2
            || collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.GoalP1
            || collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.GoalP2)
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.Bumper)
        {
            //rb.velocity = Vector2.zero;
            //rb.AddForce(-collision.contacts[0].normal * bouncePlayerPower * .001f, ForceMode2D.Impulse);
            var speed = lastVelocity.magnitude;
            var direction = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);

            rb.velocity = direction * Mathf.Max(speed, 0f);
            //rb.freezeRotation = false;

            Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 5000f);
            transform.localEulerAngles += new Vector3(0,0,90);
        }

        if ((collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.P1 // P1 & P2
            && gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.BulletP2) 
            || (collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.P2
            && gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.BulletP1))
        {
            if (GameParameters.instance.Mode == GameParameters.WhichMode.Possession || GameParameters.instance.Mode == GameParameters.WhichMode.Soccer)
            {
                collision.gameObject.GetComponent<PlayerMovement>().LaunchBounceBullet();
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(-collision.contacts[0].normal, ForceMode2D.Impulse);
            }
            Destroy(gameObject);
        }

        if (collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.Ball) // Ball
           Destroy(gameObject);
    }

    private Vector2 lastVelocity;
    private void Update()
    {
        lastVelocity = rb.velocity;
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.Outside
    //        || collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.P1
    //        || collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.P2)
    //    {
    //        Destroy(gameObject);
    //    }
    //    else
    //        return;
    //}
}
