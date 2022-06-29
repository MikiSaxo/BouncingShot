using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Rigidbody2D rb;
    public SpriteRenderer spr;
    public ShakeCamera shakeCamera;

    [SerializeField] int bumperPower, bulletPower, color;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.Outside)
        {
            gameObject.GetComponent<ReboundAnimation>().StartBounce();
        }

        if (collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.Bumper)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(-collision.contacts[0].normal * bumperPower, ForceMode2D.Impulse);
            collision.gameObject.GetComponent<ReboundAnimation>().StartBounce();
            gameObject.GetComponent<ReboundAnimation>().StartBounce();
        }

        if (collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.BulletP1)
        {
            ChangeColor(1);
            rb.AddForce(collision.contacts[0].normal * bulletPower, ForceMode2D.Impulse);
            gameObject.GetComponent<ReboundAnimation>().StartBounce();
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.BulletP2)
        {
            ChangeColor(2);
            rb.AddForce(collision.contacts[0].normal * bulletPower, ForceMode2D.Impulse);
            gameObject.GetComponent<ReboundAnimation>().StartBounce();
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.P1 && color != 1)
        {
            Manager.instance.WhichBallTouches(1, 2);
            RipplePostProcessor.instance.RippleEffect(transform.position);
            shakeCamera.CamShake();
        }
        else if (collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.P2 && color != 2)
        {
            Manager.instance.WhichBallTouches(2, 1);
            RipplePostProcessor.instance.RippleEffect(transform.position);
            shakeCamera.CamShake();
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
