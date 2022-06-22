using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionBullet : MonoBehaviour
{
    /*public Collider Trigger;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("BulletP1") && gameObject.tag == "BulletP2")
        {
            print("P2 rencontre P1");
            if (collision.gameObject.GetComponent<Bullet>().timer > gameObject.GetComponentInParent<Bullet>().timer)
            {
                print("P2 > P1");
                Destroy(gameObject);
            }
            else if (collision.gameObject.GetComponent<Bullet>().timer == gameObject.GetComponentInParent<Bullet>().timer)
            {
                Destroy(collision.gameObject);
                Destroy(gameObject);
            }
        }

        if (collision.transform.CompareTag("BulletP2") && gameObject.tag == "BulletP1")
        {
            print("P1 rencontre P2");
            if (collision.gameObject.GetComponent<Bullet>().timer > gameObject.GetComponentInParent<Bullet>().timer)
            {
                print("P1 > P2");
                Destroy(gameObject);
            }
            else if (collision.gameObject.GetComponent<Bullet>().timer == gameObject.GetComponentInParent<Bullet>().timer)
            {
                Destroy(collision.gameObject);
                Destroy(gameObject);
            }
        }
    }*/
}
