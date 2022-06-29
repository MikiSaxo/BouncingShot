using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] float Speed;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] SpriteRenderer MainSprite;
    [SerializeField] int bouncePower;

    [HideInInspector] public bool CanMove;

    Vector2 movementInput = Vector2.zero;

    private void Start()
    {
        print("P" + Manager.instance.NbOfPlayer);
        Manager.instance.Players.Add(gameObject);
        gameObject.GetComponentInChildren<CursorMovement>().WhichPlayer = Manager.instance.NbOfPlayer;
        if (Manager.instance.NbOfPlayer == 1)
        {
            MainSprite.color = Color.cyan;
            transform.position = Manager.instance.SpawnPoints[0].position;
            Manager.instance.NbOfPlayer++;
            Manager.instance.playerScoreVisu[0].GetComponent<FollowObject>().target = gameObject.transform;
        }
        else
        {
            gameObject.GetComponent<WhoAreYou>().ChoisiBieng = WhoAreYou.ChooseYourChampion.P2;
            MainSprite.color = Color.red;
            transform.position = Manager.instance.SpawnPoints[1].position;
            Manager.instance.LaunchGame();
            Manager.instance.playerScoreVisu[1].GetComponent<FollowObject>().target = gameObject.transform;
        }
    }

    void Update()
    {
        if (movementInput != Vector2.zero && CanMove)
            Movement();

        if (!CanMove)
        {
            Destroy(GameObject.Find("Bullet(Clone)"));
        }
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    void Movement()
    {
        Vector2 m2 = new Vector2(movementInput.x, movementInput.y) * Speed;
        //if (!CanMove)
        //    m2 = Vector2.zero;
        rb.velocity = m2;
    }


    public void LaunchBounceBullet()
    {
        CanMove = false;
        StartCoroutine(TakeAShot());
    }
    IEnumerator TakeAShot()
    {
        //rb.AddForce(collision.contacts[0].normal * bouncePower, ForceMode2D.Impulse);
        yield return new WaitForSeconds(.5f);
        CanMove = true;
    }
}
