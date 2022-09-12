using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    [SerializeField] private GameObject[] domiBG;
    [SerializeField] private GameObject[] leftSquare;
    [SerializeField] private GameObject[] rightSquare;
    private float fadingSpeed = 0.05f;
    [SerializeField] private bool stopCorou = false;

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

        statesColor[1] = GameParameters.instance.blueColorToChoose[GameParameters.instance.BlueColors];
        statesColor[2 ] = GameParameters.instance.redColorToChoose[GameParameters.instance.RedColors];

        if (GameParameters.instance.Mode == GameParameters.WhichMode.Soccer)
        {
            goals[0].SetActive(true);
            goals[1].SetActive(false);
            goals[2].SetActive(true);

            Ball.transform.localScale = soccerSize;
        }

        if (GameParameters.instance.Mode == GameParameters.WhichMode.Domination)
        {
            domiBG[0].SetActive(true);
            domiBG[1].SetActive(true);
            //domiBG[0].GetComponent<Image>().color = statesColor[3];
            //domiBG[1].GetComponent<Image>().color = statesColor[4];
            for (int i = 0; i < leftSquare.Length; i++)
            {
                leftSquare[i].GetComponent<Image>().color = statesColor[3];
            }
            for (int i = 0; i < rightSquare.Length; i++)
            {
                rightSquare[i].GetComponent<Image>().color = statesColor[4];
            }
        }

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
            ChangeBordersColor(statesColor[whoTouch], true);
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

        if (borders[0].color.a >= 0.1f)
        {
            if(GameParameters.instance.Mode != GameParameters.WhichMode.Domination && GameParameters.instance.Mode != GameParameters.WhichMode.Possession)
            {
                print("c chiant la");
                StartCoroutine(TransiResetColor());
            }
        }
    }

    public void ChangeBordersColor(Color _color, bool needReset)
    {
        for (int i = 0; i < borders.Length; i++)
        {
            borders[i].GetComponent<Image>().color = _color;
        }

        if (needReset)
        {
            StartCoroutine(TransiResetColor());
            print(borders[0].color.a);
        }
    }

    IEnumerator TransiResetColor()
    {
        print("salut c la transi");
        stopCorou = true;
        yield return new WaitForSeconds(.1f);
        stopCorou = false;
        StartCoroutine(ResetColor());
    }

    IEnumerator test()
    {
        var j = 0;
        for (int i = 0; i < 100; i++)
        {
            if (stopCorou)
                break;

            j++;
            print(j);
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator ResetColor()
    {
        if (stopCorou)
            print("ça pu");

        for (float i = 1f; i >= 0; i -= fadingSpeed)
        {
            for (int j = 0; j < borders.Length; j++)
            {
                if (stopCorou)
                {
                    print("j'ai stop");
                    j = borders.Length;
                    break;
                }

                var tempColor = borders[j].color;
                tempColor.a = i;
                borders[j].color = tempColor;
            }
            if (stopCorou)
            {
                print("j'ai arrete");
                i = 0;
                break;
            }
            yield return new WaitForSeconds(fadingSpeed);
        }
    }

    void SpawnBordersColors()
    {
        for (int i = 0; i < borders.Length / 2; i++)
        {
            borders[i].GetComponent<Image>().color = statesColor[2];
        }
        for (int i = borders.Length / 2; i < borders.Length; i++)
        {
            borders[i].GetComponent<Image>().color = statesColor[1];
        }
    }
}
