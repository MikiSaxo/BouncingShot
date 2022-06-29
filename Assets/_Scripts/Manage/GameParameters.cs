using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameParameters : MonoBehaviour
{
    public WhichMode Mode;

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

    public enum WhichMode
    {
        Normal,
        Blitz,
        Domination,
        Possession
    }

    public void ChooseMode(int mode)
    {
        Mode = (WhichMode)mode;
        print(Mode);
        SceneManager.LoadScene(1);
    }

}
