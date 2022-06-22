using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Manager : MonoBehaviour
{
    public List<GameObject> Players = new List<GameObject>(2);
    public GameObject Ball;
    public Transform[] SpawnPoints;
    public TextMeshProUGUI[] TextScores;
    public int[] NbScores;
    [HideInInspector]
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
    }

    public void WhichBallTouches(int whichPlayer)
    {
        NbScores[whichPlayer]++;
        TextScores[whichPlayer].text = NbScores[whichPlayer].ToString();

        Ball.transform.position = SpawnPoints[whichPlayer+2].position;
        Ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;


        TextScores[2].gameObject.SetActive(true);
        if (whichPlayer == 0)
        {
            TextScores[2].text = "Red Scores";
            TextScores[2].color = Color.red;
        }
        else
        {
            TextScores[2].text = "Blue Scores";
            TextScores[2].color = Color.cyan;
        }

        StartCoroutine(Replace());
    }

    IEnumerator Replace()
    {
        Players[0].transform.position = SpawnPoints[0].position;
        Players[1].transform.position = SpawnPoints[1].position;
        

        Players[0].GetComponent<PlayerMovement>().CanMove = false;
        Players[1].GetComponent<PlayerMovement>().CanMove = false;
        yield return new WaitForSeconds(1f);
        TextScores[2].gameObject.SetActive(false);
        Players[0].GetComponent<PlayerMovement>().CanMove = true;
        Players[1].GetComponent<PlayerMovement>().CanMove = true;
    }
}
