using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] float Speed;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] SpriteRenderer MainSprite;
    [SerializeField] int maxDashPower;
    [SerializeField] Animator DashAnim;
    private int dashPower;
    private bool dash, isCooldown, dashAnim, pause, validate;
    public bool IsPaused;

    private float nextDash = 1f;
    [SerializeField] float dashRate = 1f;

    [SerializeField] float[] vib_Spawn;
    [SerializeField] float[] vib_Dash;

    private float timeScaleInZone = .01f;
    const float timeScaleSlowMo = .02f;
    private float canPauseLaunch;
    private float canPauseLeave;

    [HideInInspector] public bool CanMove;
    [HideInInspector] public bool isShotByBumper;

    Vector2 movementInput = Vector2.zero;

    private void Start()
    {
        dashPower = 1;
        print("P" + Manager.instance.NbOfPlayer);
        Manager.instance.Players.Add(gameObject);
        gameObject.GetComponentInChildren<CursorMovement>().WhichPlayer = Manager.instance.NbOfPlayer;
        if (Manager.instance.NbOfPlayer == 1)
        {
            MainSprite.color = Manager.instance.statesColor[1];
            gameObject.GetComponent<TrailRenderer>().startColor = Manager.instance.statesColor[1];
            gameObject.GetComponent<TrailRenderer>().endColor = Manager.instance.statesColor[1];
            transform.position = Manager.instance.SpawnPoints[0].position;
            Manager.instance.PlayerOneHasJoinded();
            Manager.instance.NbOfPlayer++;
            Manager.instance.playerScoreVisu[0].GetComponent<FollowObject>().target = gameObject.transform;

            gameObject.GetComponent<VibrateController>().playerIndex = XInputDotNetPure.PlayerIndex.One;
            gameObject.GetComponent<VibrateController>().StartVibration(vib_Spawn[0], vib_Spawn[1], vib_Spawn[2]);
            //print(Gamepad.current);
        }
        else
        {
            gameObject.GetComponent<WhoAreYou>().ChoisiBieng = WhoAreYou.ChooseYourChampion.P2;
            MainSprite.color = Manager.instance.statesColor[2];
            gameObject.GetComponent<TrailRenderer>().startColor = Manager.instance.statesColor[2];
            gameObject.GetComponent<TrailRenderer>().endColor = Manager.instance.statesColor[2];
            transform.position = Manager.instance.SpawnPoints[1].position;
            Manager.instance.LaunchGame();
            Manager.instance.playerScoreVisu[1].GetComponent<FollowObject>().target = gameObject.transform;

            gameObject.GetComponent<VibrateController>().playerIndex = XInputDotNetPure.PlayerIndex.Two;
            gameObject.GetComponent<VibrateController>().StartVibration(vib_Spawn[0], vib_Spawn[1], vib_Spawn[2]);
            //print(Gamepad.current);

        }
    }

    void Update()
    {
        if (movementInput != Vector2.zero && CanMove)
            Movement();

        if (!CanMove && Manager.instance.IsReplacing)
        {
            Destroy(GameObject.Find("Bullet(Clone)"));
            Manager.instance.IsReplacing = false;
        }

        if (pause)
        {
            if (!IsPaused && canPauseLaunch >= 1f)
                LaunchPause();
            else if(IsPaused && canPauseLeave >= 1f)
                LeavePause();
        }
        if(!IsPaused)
            canPauseLaunch += Time.deltaTime;
        else
            canPauseLeave += Time.deltaTime * 100;

        if (isCooldown == false)
        {
            if (dash)
            {
                isCooldown = true;
                nextDash = dashRate;
                Dash();
            }
        }

        if (isCooldown)
        {
            nextDash -= Time.deltaTime;
            if (nextDash <= 0)
            {
                isCooldown = false;
            }
        }
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        dash = context.action.triggered;
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        pause = context.action.triggered;
    }

    void LaunchPause()
    {
        if (Manager.instance.IsGameEnded)
            return;

        Time.timeScale = timeScaleInZone;
        Time.fixedDeltaTime = timeScaleSlowMo * Time.timeScale;
        gameObject.GetComponent<VibrateController>().StopVibra();
        CanMove = false;
        gameObject.GetComponent<CursorMovement>().CanShoot(false);
        IsPaused = true;
        Manager.instance.EnablePause(true);
        canPauseLeave = 0;
    }

    public void LeavePause()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = timeScaleSlowMo * Time.timeScale;
        Manager.instance.EnablePause(false);
        CanMove = true;
        gameObject.GetComponent<CursorMovement>().CanShoot(true);
        IsPaused = false;
        canPauseLaunch = 0;
        gameObject.GetComponent<VibrateController>().StopVibra();
    }

    void Dash()
    {
        if (movementInput != Vector2.zero)
        {
            gameObject.GetComponent<VibrateController>().StartVibration(vib_Dash[0], vib_Dash[1], vib_Dash[2]);
            dashPower = maxDashPower;
        }
    }

    void Movement()
    {
        Vector2 m2 = new Vector2(movementInput.x, movementInput.y) * Speed * dashPower;
        rb.velocity = m2;
        if (dashPower == maxDashPower)
            StartCoroutine(ResetDash());
    }

    IEnumerator ResetDash()
    {
        yield return new WaitForSeconds(.1f);
        dashPower = 1;
        if (!dashAnim)
        {
            DashAnim.SetTrigger("LaunchDash");
            dashAnim = true;
        }
        yield return new WaitForSeconds(.1f);
        dashAnim = false;
    }

    public void LaunchBounceBullet()
    {
        gameObject.GetComponent<ReboundAnimation>().StartBounce();
        CanMove = false;
        StartCoroutine(TakeAShot());
    }
    IEnumerator TakeAShot()
    {
        var getColor = MainSprite.color;
        MainSprite.color = Color.white;
        if (!isShotByBumper)
            yield return new WaitForSeconds(.5f);
        else if (isShotByBumper)
        {
            yield return new WaitForSeconds(.3f);
            isShotByBumper = false;
        }
        MainSprite.color = getColor;
        CanMove = true;
    }
}
