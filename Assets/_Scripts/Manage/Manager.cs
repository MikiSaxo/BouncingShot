using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
    [SerializeField] GameObject[] goals;
    [SerializeField] Vector3 soccerSize;
    public Color[] statesColor;
    public Image[] borders;
    private float timeFadeBorders = 0f;

    const int anounceText = 0;

    public static Manager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de GameManager dans la sc�ne");
            return;
        }
        instance = this;
    }

    private void Start()
    {
        TextScores[0].gameObject.SetActive(true);
        TextScores[0].text = Phrases[0];
        TextScores[anounceText].color = Color.yellow;

        if (GameParameters.instance.Mode == GameParameters.WhichMode.Soccer)
        {
            goals[0].SetActive(true);
            goals[1].SetActive(false);
            goals[2].SetActive(true);

            Ball.transform.localScale = soccerSize;
        }
        timeFadeBorders = 3;
        SpawnBordersColors();
    }

    public void LaunchGame()
    {
        StartCoroutine(Decompte());
    }
    public void WhichBallTouches(int wasTouch, int whoTouch)
    {
        NbScores[whoTouch]++;
        TextScores[whoTouch].text = NbScores[whoTouch].ToString();
        if (GameParameters.instance.Mode == GameParameters.WhichMode.Normal || GameParameters.instance.Mode == GameParameters.WhichMode.Soccer)
        {
            Ball.transform.position = SpawnPoints[wasTouch + 1].position;
            Ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

            if (GameParameters.instance.Mode == GameParameters.WhichMode.Normal)
            {
                Ball.GetComponent<Ball>().ChangeColor(wasTouch);
                Ball.GetComponent<Ball>().bulletPower = Ball.GetComponent<Ball>().bullerPowerNormal;
                Ball.GetComponent<Rigidbody2D>().drag = 1;
            }

            TextScores[anounceText].gameObject.SetActive(true);
            if (whoTouch == 2)
            {
                TextScores[anounceText].text = Phrases[1];
                TextScores[anounceText].color = statesColor[2];
            }
            else
            {
                TextScores[anounceText].text = Phrases[2];
                TextScores[anounceText].color = statesColor[1];
            }

            StartCoroutine(Replace());
        }

        if (GameParameters.instance.Mode == GameParameters.WhichMode.Blitz)
        {
            StartCoroutine(ReplaceBlitz());
        }
    }

    public void ScorePossession(int whoNotTouch)
    {
        NbScoresPoss[whoNotTouch] += Time.deltaTime;
        int score = (int)NbScoresPoss[whoNotTouch];
        TextScores[whoNotTouch].text = score.ToString();
    }


    IEnumerator ReplaceBlitz()
    {
        Ball.SetActive(false);
        yield return new WaitForSeconds(.5f);
        Ball.SetActive(true);
        Ball.transform.position = SpawnPoints[4].position;
        Ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        Ball.GetComponent<Ball>().ChangeColor(0);
    }

    IEnumerator Replace()
    {
        Ball.SetActive(false);

        Players[0].GetComponent<PlayerMovement>().CanMove = false;
        Players[1].GetComponent<PlayerMovement>().CanMove = false;
        Players[0].GetComponent<CursorMovement>().IsLock = true;
        Players[1].GetComponent<CursorMovement>().IsLock = true;


        Players[0].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        Players[1].GetComponent<Rigidbody2D>().velocity = Vector2.zero;


        Players[0].transform.position = SpawnPoints[0].position;
        Players[1].transform.position = SpawnPoints[1].position;

        yield return new WaitForSeconds(.1f);
        Ball.SetActive(true);
        yield return new WaitForSeconds(.9f);
        TextScores[anounceText].gameObject.SetActive(false);
        Players[0].GetComponent<CursorMovement>().IsLock = false;
        Players[1].GetComponent<CursorMovement>().IsLock = false;

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
        StartCoroutine(ResetColor());
    }

    public void ChangeBordersColor(Color _color)
    {
        for (int i = 0; i < borders.Length; i++)
        {
            borders[i].GetComponent<Image>().color = _color;
        }
        StartCoroutine(ResetColor());
    }
    private float fadingSpeed = 0.05f;

    IEnumerator ResetColor()
    {
        for (float i = 1f; i >= 0; i -= fadingSpeed)
        {
            for (int j = 0; j < borders.Length; j++)
            {
                var tempColor = borders[j].color;
                tempColor.a = i;
                borders[j].color = tempColor;
            }
            yield return new WaitForSeconds(fadingSpeed);
        }
    }

    void SpawnBordersColors()
    {
        for (int i = 0; i < borders.Length/2; i++)
        {
            borders[i].GetComponent<Image>().color = statesColor[2];
        }
        for (int i = borders.Length / 2; i < borders.Length; i++)
        {
            borders[i].GetComponent<Image>().color = statesColor[1];
        }
        //StartCoroutine(ResetColor());
    }
}
