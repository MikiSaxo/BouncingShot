using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanShootWall : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.Outside)
            gameObject.GetComponentInParent<CursorMovement>().CanShoot(false);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.Outside)
            gameObject.GetComponentInParent<CursorMovement>().CanShoot(true);
    }
}
