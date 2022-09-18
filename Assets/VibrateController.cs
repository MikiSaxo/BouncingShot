using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using XInputDotNetPure;
using UnityEngine.InputSystem;



public class VibrateController : MonoBehaviour
{
    public PlayerIndex playerIndex;
    [SerializeField] GameObject vib;
    Gamepad currentt;
    GamePadState state;
    GamePadState prevState;

    public void StartVibration(float _left, float _right, float _duration)
    {
        if (!gameObject.GetComponent<PlayerMovement>().IsPaused)
        {
            GamePad.SetVibration(playerIndex, _left, _right);
            //currentt.SetMotorSpeeds(_left, _right);

            vib.transform.DOScale(Vector3.zero, _duration).OnComplete(StopVibra);
        }
    }

    public void StopVibra()
    {
        GamePad.SetVibration(playerIndex, 0, 0f);
        //currentt.SetMotorSpeeds(0f, 0f);
    }
}
