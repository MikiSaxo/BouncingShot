using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;


public class GameParameters : MonoBehaviour
{
    public WhichMode Mode;
    public int NbOfPlayers;
    public int RedColors;
    public int BlueColors;
    public Color[] blueColorToChoose;
    public Color[] redColorToChoose;
    public int MapIndex;


    public enum WhichMode
    {
        Normal,
        Blitz,
        Domination,
        Possession,
        Soccer
    }

    public static GameParameters instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de GameManager dans la scène");
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (MenuSelection.Instance != null)
        {
            for (int i = 0; i < blueColorToChoose.Length; i++)
            {
                MenuSelection.Instance.ChangeBlueColor(i, blueColorToChoose[i]);
            }
            for (int i = 0; i < redColorToChoose.Length; i++)
            {
                MenuSelection.Instance.ChangeRedColor(i, redColorToChoose[i]);
            }
        }

        Manager.DestroyGameParameters += OnDestroyEndGame;
    }

    public void ChooseMode(int mode)
    {
        Mode = (WhichMode)mode;
        print(Mode);
    }

    public void ChooseNbPlayers(int howMany)
    {
        NbOfPlayers = howMany;
        print(NbOfPlayers);
    }

    public void ChooseBlueColor(int whichColor)
    {
        BlueColors = whichColor;
    }

    public void ChooseRedColor(int whichColor)
    {
        RedColors = whichColor;
    }

    public void ChooseMap(int index)
    {
        MapIndex = index-1;
    }

    private void OnDestroyEndGame()
    {
        Destroy(gameObject);
    }

    private void OnDisable()
    {
        Manager.DestroyGameParameters -= OnDestroyEndGame;
    }
}
