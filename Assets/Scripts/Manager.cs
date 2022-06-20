using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public Transform[] SpawnPoints;

    public int NbOfPlayer;

    public static Manager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de GameManager dans la scène");
            return;
        }
        instance = this;
        /*if (Abilities.instance.nbPlayer == 1)
        {
            rend.color = Color.red;
            gameObject.tag = "player1";
            colorRing.tag = "player1";
        }
        if (Abilities.instance.nbPlayer == 2)
        {
            rend.color = new Color(0, 0.5f, 1, 1);
            gameObject.tag = "player2";
            colorRing.tag = "player2";
        }*/
    }
}
