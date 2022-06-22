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

    const int anounceText = 0;

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

    public void WhichBallTouches(int wasTouch, int whoTouch)
    {
        NbScores[whoTouch]++;
        TextScores[whoTouch].text = NbScores[whoTouch].ToString();

        Ball.transform.position = SpawnPoints[wasTouch + 1].position;
        Ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        Ball.GetComponent<Ball>().ChangeColor(wasTouch);


        TextScores[anounceText].gameObject.SetActive(true);
        if (whoTouch == 2)
        {
            TextScores[anounceText].text = "Red Scores";
            TextScores[anounceText].color = Color.red;
        }
        else
        {
            TextScores[anounceText].text = "Blue Scores";
            TextScores[anounceText].color = Color.cyan;
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
        TextScores[anounceText].gameObject.SetActive(false);
        Players[0].GetComponent<PlayerMovement>().CanMove = true;
        Players[1].GetComponent<PlayerMovement>().CanMove = true;
    }
}
