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
        rb.velocity = transform.right * Speed;// * (timer * Speed / 2);
        //Child.tag = tag;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.Outside //Outside & Bumper
            || collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.Bumper)
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.BulletP1 // Bullets
            || collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.BulletP2)// || collision.transform.CompareTag("Ball"))
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.P1 // P1 & P2
            || collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.P2)
        {
            if (GameParameters.instance.Mode == GameParameters.WhichMode.Possession)
            {
                collision.gameObject.GetComponent<PlayerMovement>().LaunchBounceBullet();
                //StartCoroutine(TakeAShot(collision));
                //collision.gameObject.GetComponent<PlayerMovement>().LaunchBounceBullet(gameObject.GetComponent<Collision2D>());
                //collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(-collision.contacts[0].normal * bouncePlayerPower, ForceMode2D.Impulse);
            }
            Destroy(gameObject);
        }

        //Si en mode Possession pour détruire la bullet quand elle touche la balle
        if (collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.Ball && GameParameters.instance.Mode == GameParameters.WhichMode.Possession)
            Destroy(gameObject);
    }

    IEnumerator TakeAShot(Collision2D collision)
    {
        yield return new WaitForSeconds(.01f);
        //collision.gameObject.GetComponent<PlayerMovement>().movementInput = Vector2.zero;
        collision.gameObject.GetComponent<Rigidbody2D>().AddForce(-collision.contacts[0].normal * bouncePlayerPower, ForceMode2D.Impulse);
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
    }
}
