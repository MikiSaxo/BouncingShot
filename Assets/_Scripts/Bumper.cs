using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour
{
    [SerializeField] int bumperPlayerPower;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.P1 || collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.P2)
        {
            collision.gameObject.GetComponent<PlayerMovement>().isShotByBumper = true;
            collision.gameObject.GetComponent<PlayerMovement>().LaunchBounceBullet();
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(-collision.contacts[0].normal * bumperPlayerPower, ForceMode2D.Impulse);
        }
    }
}
