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
    int dashPower;
    bool dash, isCooldown, dashAnim;

    float nextDash = 1f;
    [SerializeField] float dashRate = 1f;

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
            Manager.instance.NbOfPlayer++;
            Manager.instance.playerScoreVisu[0].GetComponent<FollowObject>().target = gameObject.transform;
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

    void Dash()
    {
        if(movementInput != Vector2.zero)
            dashPower = maxDashPower;
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
        CanMove = false;
        StartCoroutine(TakeAShot());
    }
    IEnumerator TakeAShot()
    {
        if(!isShotByBumper)
            yield return new WaitForSeconds(.5f);
        else if (isShotByBumper)
        {
            yield return new WaitForSeconds(.3f);
            isShotByBumper = false;
        }
        CanMove = true;
    }
}
