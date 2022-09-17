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

    [SerializeField] GameObject fx_TouchBall;
    [SerializeField] GameObject fx_TouchPlayerBullet;
    [SerializeField] GameObject fx_BulletBullet;
    [SerializeField] GameObject fx_Shoot;
    private Vector3 transferPosition;

    void Start()
    {
        rb.velocity = transform.right * Speed;
        transferPosition = new Vector3(Manager.instance.Ball.transform.position.x, Manager.instance.Ball.transform.position.y, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.Outside //Outside & Goals & Bullets
            || collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.GoalP1
            || collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.GoalP2)
        {
            var transferPosition = new Vector3(transform.position.x, transform.position.y, 0);
            Instantiate(fx_TouchPlayerBullet, transferPosition, collision.gameObject.transform.rotation);
            Destroy(gameObject);
        }

        if (collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.BulletP1
            || collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.BulletP2)
        {
            var transferPosition = new Vector3(transform.position.x, transform.position.y, 0);
            Instantiate(fx_BulletBullet, transferPosition, transform.rotation);
            Destroy(gameObject);
        }

        if (collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.Bumper)
        {
            var transferPosition = new Vector3(transform.position.x, transform.position.y, 0);
            Instantiate(fx_TouchPlayerBullet, transferPosition, collision.gameObject.transform.rotation);

            var speed = lastVelocity.magnitude;
            var direction = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);

            rb.velocity = direction * Mathf.Max(speed, 0f);

            Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 5000f);
            transform.localEulerAngles += new Vector3(0, 0, 90);
        }

        if ((collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.P1 // Bullet qui touche un adversaire
            && gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.BulletP2)
            || (collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.P2
            && gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.BulletP1))
        {
            collision.gameObject.GetComponent<PlayerMovement>().LaunchBounceBullet();
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(-collision.contacts[0].normal * bouncePlayerPower, ForceMode2D.Impulse);

            var transferPosition = new Vector3(transform.position.x - collision.contacts[0].normal.x * .5f, transform.position.y - collision.contacts[0].normal.y * .5f, 0);
            Instantiate(fx_TouchPlayerBullet, transferPosition, collision.gameObject.transform.rotation);

            Destroy(gameObject);
        }
        else if ((collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.P1 // Bullet qui touche son pote
            && gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.BulletP1)
            || (collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.P2
            && gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.BulletP2))
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.Ball) // Ball
        {
            Instantiate(fx_TouchBall, transferPosition, transform.rotation);
            Destroy(gameObject);
        }
    }

    private Vector2 lastVelocity;
    private void Update()
    {
        lastVelocity = rb.velocity;
    }
}
