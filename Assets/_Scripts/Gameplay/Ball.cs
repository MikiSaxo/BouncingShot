using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] SpriteRenderer spr;
    public ShakeCamera shakeCamera;
    [SerializeField] GameObject Child;

    float nextReset = 1f;
    bool isCooldown;
    public float ResetRate = 1f;

    [SerializeField] int bumperPower, bulletPower, color;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.Outside) //Outside
        {
            StartRebound();
        }

        if (collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.Bumper) //Bumper
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(-collision.contacts[0].normal * bumperPower, ForceMode2D.Impulse);
            collision.gameObject.GetComponent<ReboundAnimation>().StartBounce();
            StartRebound();
        }

        if (GameParameters.instance.Mode != GameParameters.WhichMode.Possession)
        {
            if (collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.BulletP1) //BulletP1
            {
                if (GameParameters.instance.Mode != GameParameters.WhichMode.Domination)
                    ChangeColor(1);
                rb.AddForce(collision.contacts[0].normal * bulletPower, ForceMode2D.Impulse);
                StartRebound();
                Destroy(collision.gameObject);
            }
            if (collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.BulletP2) //BulletP2
            {
                if (GameParameters.instance.Mode != GameParameters.WhichMode.Domination)
                    ChangeColor(2);
                rb.AddForce(collision.contacts[0].normal * bulletPower, ForceMode2D.Impulse);
                StartRebound();
                Destroy(collision.gameObject);
            } 
            if (collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.P1 && color != 1) //P1
            {
                if (GameParameters.instance.Mode != GameParameters.WhichMode.Domination)
                    Manager.instance.WhichBallTouches(1, 2);
                if (GameParameters.instance.Mode != GameParameters.WhichMode.Possession)
                    RipplePostProcessor.instance.RippleEffect(transform.position);
                shakeCamera.CamShake();
            } 
            else if (collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.P2 && color != 2) //P2
            {
                if (GameParameters.instance.Mode != GameParameters.WhichMode.Domination)
                    Manager.instance.WhichBallTouches(2, 1);
                if (GameParameters.instance.Mode != GameParameters.WhichMode.Possession)
                    RipplePostProcessor.instance.RippleEffect(transform.position);
                shakeCamera.CamShake();
            } 
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (GameParameters.instance.Mode == GameParameters.WhichMode.Possession)
        {
            if (collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.P2 
                && collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.P1)
                ChangeColor(0);
            else if (collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.P1)
            {
                ChangeColor(1);
                isCooldown = true;
            }
            else if (collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.P2)
            {
                ChangeColor(2);
                isCooldown = true;
            }
            else
                ChangeColor(0);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (GameParameters.instance.Mode == GameParameters.WhichMode.Possession)
            isCooldown = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        print(collision);
        if (GameParameters.instance.Mode == GameParameters.WhichMode.Domination)
        {
            if (collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.CampP2
                && collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.CampP1)
                ChangeColor(0);
            else if (collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.CampP1)
                ChangeColor(1);
            else if (collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.CampP2)
                ChangeColor(2);
            else
                ChangeColor(0);
        }
    }

    private void Update()
    {
        if (GameParameters.instance.Mode == GameParameters.WhichMode.Possession)
        {
            if (isCooldown == false)
            {
                isCooldown = true;
                nextReset = ResetRate;
            }

            if (isCooldown)
            {
                nextReset -= Time.deltaTime;
                if (nextReset <= 0)
                {
                    isCooldown = false;
                    ChangeColor(0);
                }
            }
        }
        
        if (GameParameters.instance.Mode == GameParameters.WhichMode.Possession || GameParameters.instance.Mode == GameParameters.WhichMode.Domination)
        {
            if (color == 1)
                Manager.instance.ScorePossession(1);
            if (color == 2)
                Manager.instance.ScorePossession(2);
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

    void StartRebound()
    {
        Child.GetComponent<ReboundAnimation>().StartBounce();
    }
}
