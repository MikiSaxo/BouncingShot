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
    public float[] NbScoresPoss;
    [SerializeField] string[] Phrases;
    [HideInInspector] public int NbOfPlayer;
    [SerializeField] float timeForDecompteInSec;
    [SerializeField] int nbDecompte;
    public GameObject[] playerScoreVisu;

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

    private void Start()
    {
        TextScores[0].gameObject.SetActive(true);
        TextScores[0].text = Phrases[0];
        TextScores[anounceText].color = Color.yellow;
    }

    public void LaunchGame()
    {
        StartCoroutine(Decompte());
    }
    public void WhichBallTouches(int wasTouch, int whoTouch)
    {
        NbScores[whoTouch]++;
        TextScores[whoTouch].text = NbScores[whoTouch].ToString();

        if (GameParameters.instance.Mode == GameParameters.WhichMode.Normal)
        {
            Ball.transform.position = SpawnPoints[wasTouch + 1].position;
            Ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            Ball.GetComponent<Ball>().ChangeColor(wasTouch);

            TextScores[anounceText].gameObject.SetActive(true);
            if (whoTouch == 2)
            {
                TextScores[anounceText].text = Phrases[1];
                TextScores[anounceText].color = Color.red;
            }
            else
            {
                TextScores[anounceText].text = Phrases[2];
                TextScores[anounceText].color = Color.cyan;
            }
            StartCoroutine(Replace());
        }

        if (GameParameters.instance.Mode == GameParameters.WhichMode.Blitz)
        {
            Ball.transform.position = SpawnPoints[4].position;
            Ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            Ball.GetComponent<Ball>().ChangeColor(0);
        }
    }

    public void ScorePossession(int whoNotTouch)
    {
        NbScoresPoss[whoNotTouch] += Time.deltaTime;
        int score = (int)NbScoresPoss[whoNotTouch];
        TextScores[whoNotTouch].text = score.ToString();
    }


    IEnumerator Replace()
    {
        Players[0].transform.position = SpawnPoints[0].position;
        Players[1].transform.position = SpawnPoints[1].position;


        Players[0].GetComponent<PlayerMovement>().CanMove = false;
        Players[1].GetComponent<PlayerMovement>().CanMove = false;
        yield return new WaitForSeconds(1f);
        TextScores[anounceText].gameObject.SetActive(false);

        StartCoroutine(Decompte());
    }

    IEnumerator Decompte()
    {
        TextScores[0].gameObject.SetActive(false);
        TextScores[3].gameObject.SetActive(true);
        for (int i = nbDecompte; i > 0; i--)
        {
            TextScores[3].text = i.ToString();
            yield return new WaitForSeconds(timeForDecompteInSec);
        }
        TextScores[3].gameObject.SetActive(false);
        Players[0].GetComponent<PlayerMovement>().CanMove = true;
        Players[1].GetComponent<PlayerMovement>().CanMove = true;
    }
}
