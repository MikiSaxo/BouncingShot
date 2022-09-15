using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] SpriteRenderer spr;
    public ShakeCamera shakeCamera;
    [SerializeField] GameObject Child;
    [SerializeField] private ShakeCamera camAnim;

    private Color[] statesColor = new Color[5];

    public float ResetRate = 1f;

    [SerializeField] int bumperPower, addBulletPower;
    [HideInInspector] public int color;
    public int bulletPower, bullerPowerNormal, bullerPowerSoccer;

    private void Start()
    {
        if (GameParameters.instance.Mode == GameParameters.WhichMode.Normal)
            bulletPower = bullerPowerNormal;
        //if (GameParameters.instance.Mode == GameParameters.WhichMode.Soccer)
        //bulletPower = bullerPowerSoccer;

        for (int i = 0; i < Manager.instance.statesColor.Length; i++)
        {
            statesColor[i] = Manager.instance.statesColor[i];
        }

        ChangeColor(0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.Outside) //Outside
        {
            StartRebound();
            camAnim.StartShakingCam(0);
        }

        if (collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.Bumper) //Bumper
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(collision.contacts[0].normal * bumperPower, ForceMode2D.Impulse);
            collision.gameObject.GetComponent<ReboundAnimation>().StartBounce();
            StartRebound();
        }


        if (collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.BulletP1) //BulletP1
        {
            if (GameParameters.instance.Mode != GameParameters.WhichMode.Domination && GameParameters.instance.Mode != GameParameters.WhichMode.Soccer)
            {
                if (color != 1)
                    ChangeColor(1);
            }
            rb.AddForce(collision.contacts[0].normal * bulletPower, ForceMode2D.Impulse);
            StartRebound();

            //if (GameParameters.instance.Mode == GameParameters.WhichMode.Possession)
            //    Manager.instance.ChangeBordersColor(statesColor[1], false);

            Destroy(collision.gameObject);
        }
        if (collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.BulletP2) //BulletP2
        {
            if (GameParameters.instance.Mode != GameParameters.WhichMode.Domination && GameParameters.instance.Mode != GameParameters.WhichMode.Soccer)
            {
                if (color != 2)
                    ChangeColor(2);
            }
            rb.AddForce(collision.contacts[0].normal * bulletPower, ForceMode2D.Impulse);
            StartRebound();

            //if (GameParameters.instance.Mode == GameParameters.WhichMode.Possession)
            //    Manager.instance.ChangeBordersColor(statesColor[1], false);

            Destroy(collision.gameObject);
        }

        if (GameParameters.instance.Mode != GameParameters.WhichMode.Soccer)
        {
            if (collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.P1 && color == 2) //P1
            {
                if (GameParameters.instance.Mode != GameParameters.WhichMode.Domination && GameParameters.instance.Mode != GameParameters.WhichMode.Possession)
                {
                    Manager.instance.WhichBallTouches(1, 2);
                    //Manager.instance.ChangeBordersColor(statesColor[1], true);
                }
                if (GameParameters.instance.Mode != GameParameters.WhichMode.Possession && GameParameters.instance.Mode != GameParameters.WhichMode.Domination)
                    RipplePostProcessor.instance.RippleEffect(transform.position);
                //shakeCamera.CamShake();
            }
            else if (collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.P2 && color == 1) //P2
            {
                if (GameParameters.instance.Mode != GameParameters.WhichMode.Domination && GameParameters.instance.Mode != GameParameters.WhichMode.Possession)
                {
                    Manager.instance.WhichBallTouches(2, 1);
                    //Manager.instance.ChangeBordersColor(statesColor[2], true);
                }
                if (GameParameters.instance.Mode != GameParameters.WhichMode.Possession && GameParameters.instance.Mode != GameParameters.WhichMode.Domination)
                    RipplePostProcessor.instance.RippleEffect(transform.position);
                //shakeCamera.CamShake();
            }
        }

        if (GameParameters.instance.Mode == GameParameters.WhichMode.Normal)
        {
            if (collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.BulletP1 || collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.BulletP2)
            {
                bulletPower += addBulletPower;
                rb.drag -= 0.02f;
            }
        }

        if (GameParameters.instance.Mode == GameParameters.WhichMode.Possession)
        {
            if (collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.P1)
            {
                if (color != 1)
                    ChangeColor(1);
                //Manager.instance.ChangeBordersColor(statesColor[1], false);
            }
            else if (collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.P2)
            {
                if (color != 2)
                    ChangeColor(2);
                //Manager.instance.ChangeBordersColor(statesColor[2], false);
            }
        }

        if (GameParameters.instance.Mode == GameParameters.WhichMode.Soccer)
        {
            if (collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.GoalP1)
            {
                Manager.instance.WhichBallTouches(1, 2);
                //Manager.instance.ChangeBordersColor(statesColor[2], false);
            }
            else if (collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.GoalP2)
            {
                Manager.instance.WhichBallTouches(2, 1);
                //Manager.instance.ChangeBordersColor(statesColor[1], false);
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
                ChangeColor(1);
            else if (collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.P2)
                ChangeColor(2);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameParameters.instance.Mode == GameParameters.WhichMode.Domination)
        {
            if (collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.CampP2
                && collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.CampP1)
                ChangeColor(0);
            else if (collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.CampP1)
            {
                if (color != 1)
                    ChangeColor(1);
                //Manager.instance.ChangeBordersColor(statesColor[1], false);
            }
            else if (collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.CampP2)
            {
                if (color != 2)
                    ChangeColor(2);
                //Manager.instance.ChangeBordersColor(statesColor[2], false);
            }
        }
    }

    private void Update()
    {
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
        {
            spr.color = statesColor[0];
            spr.gameObject.GetComponent<TrailRenderer>().startColor = Color.white;
            spr.gameObject.GetComponent<TrailRenderer>().endColor = Color.white;

            Manager.instance.ChangeBordersColor(statesColor[0], false);
        }
        if (color == 1)
        {
            spr.color = statesColor[1];
            spr.gameObject.GetComponent<TrailRenderer>().startColor = statesColor[1];
            spr.gameObject.GetComponent<TrailRenderer>().endColor = statesColor[1];
            RipplePostProcessor.instance.RippleEffect(transform.position);

            Manager.instance.ChangeBordersColor(statesColor[1], false);
        }
        if (color == 2)
        {
            spr.color = statesColor[2];
            spr.gameObject.GetComponent<TrailRenderer>().startColor = statesColor[2];
            spr.gameObject.GetComponent<TrailRenderer>().endColor = statesColor[2];
            RipplePostProcessor.instance.RippleEffect(transform.position);

            Manager.instance.ChangeBordersColor(statesColor[2], false);
        }
    }

    void StartRebound()
    {
        Child.GetComponent<ReboundAnimation>().StartBounce();
    }
}
