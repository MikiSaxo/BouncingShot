using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System;




public class Manager : MonoBehaviour
{
    public List<GameObject> Players = new List<GameObject>(2);
    public GameObject Ball;
    public Transform[] SpawnPoints;
    public TextMeshProUGUI[] TextScores;
    public int[] NbScores;
    public float[] NbScoresPoss;
    [SerializeField] int ScoreToDoNormal = 0;
    [SerializeField] int ScoreToDoBlitz = 0;
    [SerializeField] int ScoreToDoDomination = 0;
    [SerializeField] int ScoreToDoPossession = 0;
    [SerializeField] int ScoreToDoSoccer = 0;
    private bool hasPresentedScore = false;
    [SerializeField] string[] Phrases;
    [SerializeField] string[] anounce;
    [HideInInspector] public int NbOfPlayer;
    [SerializeField] float timeForDecompteInSec;
    [SerializeField] int nbDecompte;
    public GameObject[] playerScoreVisu;
    [SerializeField] GameObject[] goals;
    [SerializeField] Vector3 soccerSize;
    public Color[] statesColor;
    public Image[] borders;
    public Image[] bordersSoccer;
    [SerializeField] private GameObject[] domiBG;
    [SerializeField] private GameObject[] leftSquare;
    [SerializeField] private GameObject[] rightSquare;
    private float fadingSpeed = 0.05f;
    private bool stopCorou = false;
    [SerializeField] private GameObject superMap;
    [SerializeField] private GameObject[] maps;
    [SerializeField] private GameObject superMapSoccer;
    [SerializeField] private GameObject[] mapsSoccer;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject[] chooseButtonMain;
    [SerializeField] GameObject fx_WinConfettis;

    public bool CanLeaveAnim = false;
    public bool IsGameEnded = false;
    [SerializeField] GameObject winMenu;

    [SerializeField] GameObject invisibleFade = null;

    [HideInInspector] public bool IsReplacing = false;

    const int anounceText = 0;

    public static event Action DestroyGameParameters;

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
        statesColor[1] = GameParameters.instance.blueColorToChoose[GameParameters.instance.BlueColors];
        statesColor[2] = GameParameters.instance.redColorToChoose[GameParameters.instance.RedColors];

        TextScores[0].gameObject.SetActive(true);
        TextScores[anounceText].text = anounce[0];
        TextScores[anounceText].color = statesColor[1];

        if (GameParameters.instance.Mode != GameParameters.WhichMode.Soccer)
        {
            superMap.SetActive(true);
            superMapSoccer.SetActive(false);
            for (int i = 0; i < maps.Length; i++)
            {
                if (i != GameParameters.instance.MapIndex)
                    maps[i].gameObject.SetActive(false);
            }
        }
        else
        {
            superMap.SetActive(false);
            superMapSoccer.SetActive(true);
            for (int i = 0; i < mapsSoccer.Length; i++)
            {
                if (i != GameParameters.instance.MapIndex)
                    mapsSoccer[i].gameObject.SetActive(false);
            }
        }


        if (GameParameters.instance.Mode == GameParameters.WhichMode.Domination || GameParameters.instance.Mode == GameParameters.WhichMode.Soccer)
        {
            domiBG[0].SetActive(true);
            domiBG[1].SetActive(true);

            if (GameParameters.instance.Mode != GameParameters.WhichMode.Soccer)
            {
                for (int i = 0; i < leftSquare.Length; i++)
                {
                    leftSquare[i].GetComponent<Image>().color = statesColor[1];
                }
                for (int i = 0; i < rightSquare.Length; i++)
                {
                    rightSquare[i].GetComponent<Image>().color = statesColor[2];
                }
            }
            else
            {
                goals[0].SetActive(true);
                goals[1].GetComponent<Image>().color = statesColor[1];
                goals[2].GetComponent<Image>().color = statesColor[2];
                goals[3].SetActive(false);
                goals[4].SetActive(true);

                //Ball.transform.localScale = soccerSize;
            }
        }

        SpawnBordersColors();
    }

    public void LaunchGame()
    {
        StartCoroutine(Decompte());
    }

    public void PlayerOneHasJoinded()
    {
        TextScores[anounceText].text = anounce[1];
        TextScores[anounceText].color = statesColor[2];
    }

    public void WhichBallTouches(int wasTouch, int whoTouch)
    {
        NbScores[whoTouch]++;
        IsEndgame(NbScores[whoTouch], whoTouch);
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
            else
                Ball.GetComponent<Ball>().ChangeColor(0);

            TextScores[anounceText].gameObject.SetActive(true);

            AudioManager.Instance.PlaySound("PlayerDeath");

            if (!IsGameEnded)
            {
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
            }

            StartCoroutine(Replace());
        }

        if (GameParameters.instance.Mode == GameParameters.WhichMode.Blitz)
        {
            StartCoroutine(ReplaceBlitz());
            //ChangeBordersColor(statesColor[whoTouch], true);
        }
    }

    public void ScorePossession(int whoNotTouch)
    {
        if (!IsGameEnded)
        {
            NbScoresPoss[whoNotTouch] += Time.deltaTime;
            int score = (int)NbScoresPoss[whoNotTouch];
            TextScores[whoNotTouch].text = score.ToString();
            IsEndgame(score, whoNotTouch);
        }
    }

    IEnumerator ReplaceBlitz()
    {
        if (!IsGameEnded)
        {
            Ball.SetActive(false);
            yield return new WaitForSeconds(.5f);
            Ball.SetActive(true);
            Ball.transform.position = SpawnPoints[4].position;
            Ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            Ball.GetComponent<Ball>().bulletPower = Ball.GetComponent<Ball>().bullerPowerNormal;
            Ball.GetComponent<Rigidbody2D>().drag = 1;
            Ball.GetComponent<Ball>().ChangeColor(0);
        }
    }

    IEnumerator Replace()
    {
        if (!IsGameEnded)
        {
            Ball.SetActive(false);
            IsReplacing = true;

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
    }

    IEnumerator Decompte()
    {
        if (!IsGameEnded)
        {
            if (!hasPresentedScore)
            {
                TextScores[0].color = Color.white;
                var scoreToDraw = 0;

                if (GameParameters.instance.Mode == GameParameters.WhichMode.Normal)
                    scoreToDraw = ScoreToDoNormal;
                if (GameParameters.instance.Mode == GameParameters.WhichMode.Blitz)
                    scoreToDraw = ScoreToDoBlitz;
                if (GameParameters.instance.Mode == GameParameters.WhichMode.Domination)
                    scoreToDraw = ScoreToDoDomination;
                if (GameParameters.instance.Mode == GameParameters.WhichMode.Possession)
                    scoreToDraw = ScoreToDoPossession;
                if (GameParameters.instance.Mode == GameParameters.WhichMode.Soccer)
                    scoreToDraw = ScoreToDoSoccer;

                TextScores[0].text = $"<color=yellow><incr>{scoreToDraw}</incr></color>{Phrases[3]}";

                yield return new WaitForSeconds(1f);
                TextScores[0].transform.DOScale(Vector3.zero, .2f);
                hasPresentedScore = true;
                yield return new WaitForSeconds(.5f);
                TextScores[0].transform.DOScale(Vector3.one, 0);
            }

            TextScores[0].gameObject.SetActive(false);
            TextScores[3].gameObject.SetActive(true);
            TextScores[3].gameObject.transform.DOScale(Vector2.one, 0);

            ChangeBordersColor(statesColor[3], false);

            for (int i = nbDecompte; i > 0; i--)
            {
                TextScores[3].text = $"<shake>{i}";
                yield return new WaitForSeconds(timeForDecompteInSec);
            }
            TextScores[3].transform.DOScale(Vector3.zero, 0);
            TextScores[3].gameObject.SetActive(false);

            ChangeBordersColor(statesColor[0], false);

            Players[0].GetComponent<PlayerMovement>().CanMove = true;
            Players[1].GetComponent<PlayerMovement>().CanMove = true;
        }
    }

    public void ChangeBordersColor(Color _color, bool needReset)
    {
        if (GameParameters.instance.Mode != GameParameters.WhichMode.Soccer)
        {
            for (int i = 0; i < borders.Length; i++)
            {
                borders[i].GetComponent<Image>().color = _color;
            }
        }
        else
        {
            for (int i = 0; i < bordersSoccer.Length; i++)
            {
                bordersSoccer[i].GetComponent<Image>().color = _color;
            }
        }

        if (needReset)
            StartCoroutine(TransiResetColor());
    }

    IEnumerator TransiResetColor()
    {
        stopCorou = true;
        yield return new WaitForSeconds(.1f);
        stopCorou = false;
        StartCoroutine(ResetColor());
    }

    IEnumerator ResetColor()
    {
        for (float i = 1f; i >= 0; i -= fadingSpeed)
        {
            if (GameParameters.instance.Mode != GameParameters.WhichMode.Soccer)
            {
                for (int j = 0; j < borders.Length; j++)
                {
                    if (stopCorou)
                        break;

                    var tempColor = borders[j].color;
                    tempColor.a = i;
                    borders[j].color = tempColor;
                }
            }
            else
            {
                for (int j = 0; j < bordersSoccer.Length; j++)
                {
                    if (stopCorou)
                        break;

                    var tempColor = bordersSoccer[j].color;
                    tempColor.a = i;
                    bordersSoccer[j].color = tempColor;
                }
            }
            if (stopCorou)
                break;

            yield return new WaitForSeconds(fadingSpeed);
        }
    }

    void SpawnBordersColors()
    {
        if (GameParameters.instance.Mode != GameParameters.WhichMode.Soccer)
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
        else
        {
            for (int i = 0; i < bordersSoccer.Length / 2; i++)
            {
                bordersSoccer[i].GetComponent<Image>().color = statesColor[2];
            }
            for (int i = bordersSoccer.Length / 2; i < bordersSoccer.Length; i++)
            {
                bordersSoccer[i].GetComponent<Image>().color = statesColor[1];
            }
        }
    }

    public void EnablePause(bool which)
    {
        pauseMenu.SetActive(which);
        if (which)
            EventSystem.current.SetSelectedGameObject(chooseButtonMain[0]);
    }

    public void SwitchVibra()
    {
        print("has switch");
        if (NbOfPlayer < 2)
            return;
        if (Players[0].gameObject.GetComponent<VibrateController>().playerIndex == XInputDotNetPure.PlayerIndex.One)
        {
            Players[0].gameObject.GetComponent<VibrateController>().playerIndex = XInputDotNetPure.PlayerIndex.Two;
            Players[1].gameObject.GetComponent<VibrateController>().playerIndex = XInputDotNetPure.PlayerIndex.One;
        }
        else
        {
            Players[0].gameObject.GetComponent<VibrateController>().playerIndex = XInputDotNetPure.PlayerIndex.One;
            Players[1].gameObject.GetComponent<VibrateController>().playerIndex = XInputDotNetPure.PlayerIndex.Two;
        }
        Players[0].GetComponent<PlayerMovement>().LeavePause();
        Players[1].GetComponent<PlayerMovement>().LeavePause();
    }

    public void PressLeaveAnim()
    {
        Players[0].GetComponent<PlayerMovement>().LeavePause();
        goals[5].SetActive(false);
        invisibleFade.transform.DOScale(Vector3.zero, 1.5f).OnComplete(GoToMenu);
    }

    public void RestartGame()
    {
        invisibleFade.transform.DOScale(Vector3.zero, 1.5f).OnComplete(GoToMain);
    }

    private void GoToMenu()
    {
        DestroyGameParameters?.Invoke();
        SceneManager.LoadScene(0);
    }

    private void GoToMain()
    {
        SceneManager.LoadScene(1);
    }

    public void IsEndgame(int score, int whodwin)
    {
        if (GameParameters.instance.Mode == GameParameters.WhichMode.Normal && score >= ScoreToDoNormal)
            EndGame(whodwin);
        if (GameParameters.instance.Mode == GameParameters.WhichMode.Blitz && score >= ScoreToDoBlitz)
            EndGame(whodwin);
        if (GameParameters.instance.Mode == GameParameters.WhichMode.Domination && score >= ScoreToDoDomination)
            EndGame(whodwin);
        if (GameParameters.instance.Mode == GameParameters.WhichMode.Possession && score >= ScoreToDoPossession)
            EndGame(whodwin);
        if (GameParameters.instance.Mode == GameParameters.WhichMode.Soccer && score >= ScoreToDoSoccer)
            EndGame(whodwin);

        EventSystem.current.SetSelectedGameObject(chooseButtonMain[1]);

    }

    void EndGame(int whowin)
    {
        print("whowin " + whowin);

        ChangeBordersColor(statesColor[whowin], false);
        for (int i = 0; i < leftSquare.Length; i++)
        {
            leftSquare[i].GetComponent<Image>().color = statesColor[whowin];
        }
        for (int i = 0; i < rightSquare.Length; i++)
        {
            rightSquare[i].GetComponent<Image>().color = statesColor[whowin];
        }

        IsGameEnded = true;
        StopPlayers();
        winMenu.SetActive(true);
        TextScores[anounceText].gameObject.SetActive(true);
        Color test = statesColor[whowin];
        string colorHex = ColorUtility.ToHtmlStringRGB(test);
        TextScores[0].text = $"<color=#{colorHex}><bounce>Team {whowin}</bounce></color>{Phrases[4]}";
        fx_WinConfettis.SetActive(true);

        //Time.timeScale = .001f;
        //Time.fixedDeltaTime = .02f * Time.timeScale;
    }

    void StopPlayers()
    {
        Players[0].GetComponent<VibrateController>().StopVibra();
        Players[0].GetComponent<CursorMovement>().CanShoot(false);
        Players[0].GetComponent<PlayerMovement>().CanMove = false;

        Players[1].GetComponent<VibrateController>().StopVibra();
        Players[1].GetComponent<CursorMovement>().CanShoot(false);
        Players[1].GetComponent<PlayerMovement>().CanMove = false;
    }
}
