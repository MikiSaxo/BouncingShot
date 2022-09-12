using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SquareFeedback : MonoBehaviour
{
    [SerializeField] private Color idleColor;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.Ball)
        {
            transform.localScale = Vector3.one * 3;

            if (GameParameters.instance.Mode != GameParameters.WhichMode.Domination)
            {
                if (collision.gameObject.GetComponent<Ball>().color == 1)
                    gameObject.GetComponent<Image>().color = Manager.instance.statesColor[1];
                else if (collision.gameObject.GetComponent<Ball>().color == 2)
                    gameObject.GetComponent<Image>().color = Manager.instance.statesColor[2];
            }

            ResetScale();
        }
    }

    private void ResetScale()
    {
        transform.DOScale(Vector3.one, 2f);
    }
}
