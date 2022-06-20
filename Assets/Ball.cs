using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Rigidbody2D rb;
    [SerializeField]
    int bumperPower;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Bumper"))
        {
            //Rigidbody2D otherRb = collision.rigidbody;
            rb.AddForce(-collision.contacts[0].normal * bumperPower, ForceMode2D.Impulse);
        }

        if (collision.transform.CompareTag("P1"))
        {
            Manager.instance.WhichBallTouches(0);
        }
        else if (collision.transform.CompareTag("P2"))
            Manager.instance.WhichBallTouches(1);
    }
}
