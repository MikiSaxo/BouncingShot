using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Rigidbody2D rb;
    public SpriteRenderer spr;
    [SerializeField]
    int bumperPower, bulletPower, color;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Bumper"))
        {
            //Rigidbody2D otherRb = collision.rigidbody;
            rb.AddForce(-collision.contacts[0].normal * bumperPower, ForceMode2D.Impulse);
            ChangeColor(0);
        }


        if (collision.transform.CompareTag("P1") && color != 1)
            Manager.instance.WhichBallTouches(1, 2);
        else if (collision.transform.CompareTag("P2") && color != 2)
            Manager.instance.WhichBallTouches(2, 1);



        if (collision.transform.CompareTag("BulletP1"))
        {
            ChangeColor(1);
            //rb.velocity = Vector2.zero;
            rb.AddForce(collision.contacts[0].normal * bulletPower * collision.gameObject.GetComponent<Bullet>().timer, ForceMode2D.Impulse);
            Destroy(collision.gameObject);
        }
        if (collision.transform.CompareTag("BulletP2"))
        {
            ChangeColor(2);
            //rb.velocity = Vector2.zero;
            rb.AddForce(collision.contacts[0].normal * bulletPower * collision.gameObject.GetComponent<Bullet>().timer, ForceMode2D.Impulse);
            Destroy(collision.gameObject);
        }
    }


    public void ChangeColor(int which)
    {
        color = which;
        if (color == 0)
            spr.color = Color.white;
        if (color == 1)
            spr.color = Color.cyan;
        if (color == 2)
            spr.color = Color.red;
    }
}
