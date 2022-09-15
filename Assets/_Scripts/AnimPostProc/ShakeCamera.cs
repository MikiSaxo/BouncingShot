using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShakeCamera : MonoBehaviour
{
    //public Animator CamAnim;

    public bool Start = false;
    [SerializeField] private AnimationCurve curve;
    public float durationShaking = 1f;
    //public float durationZoom = 0f;

    public static ShakeCamera Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Start)
        {
            Start = false;
            StartCoroutine(Shaking(0));
        }
    }

    public void StartShakingCam(float _numberChoose)
    {
        StartCoroutine(Shaking(_numberChoose));
    }

    private IEnumerator Shaking(float _numberChoose)
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        var _addDuration = _numberChoose / 25 + durationShaking;

        while (elapsedTime < _addDuration)
        {
            elapsedTime += Time.deltaTime;
            float strength = curve.Evaluate(elapsedTime / _addDuration);
            transform.position = startPosition + Random.insideUnitSphere * strength;
            yield return null;
        }

        transform.position = startPosition;
    }


    //public void StartZoom(float _numberZoom)
    //{
    //    StartCoroutine(ZoomCam(_numberZoom));
    //}

    //private IEnumerator ZoomCam(float _numberZoom)
    //{
    //    gameObject.GetComponent<Camera>().DOKill();
    //    gameObject.GetComponent<Camera>().DOOrthoSize(5f - _numberZoom, .1f);
    //    yield return new WaitForSeconds(durationZoom);
    //    gameObject.GetComponent<Camera>().DOOrthoSize(5f, .05f);
    //}
}
