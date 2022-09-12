using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class FadeSystem : MonoBehaviour
{
    [SerializeField] GameObject invisible = null;
    [SerializeField] float timeFadeOn;
    [SerializeField] float timeFadeOff;

    private void Start()
    {
        invisible.transform.DOScale(Vector3.zero, 0f);
        FadeOff();
    }

    public void FadeOn(bool canLaunch)
    {
        if(canLaunch)
            invisible.transform.DOScale(Vector3.zero, timeFadeOn).OnComplete(LoadMainScene);
        else
            invisible.transform.DOScale(Vector3.zero, timeFadeOn);
    }

    public void FadeOff()
    {
        invisible.transform.DOScale(Vector3.one, timeFadeOff);
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene(1);
    }
}
