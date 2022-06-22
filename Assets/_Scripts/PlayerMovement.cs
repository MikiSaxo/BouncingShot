using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] float Speed;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] SpriteRenderer MainSprite;

    [HideInInspector] public bool CanMove;

    Vector2 movementInput = Vector2.zero;

    private void Start()
    {
        CanMove = true;
        print("P" + Manager.instance.NbOfPlayer);
        Manager.instance.Players.Add(gameObject);
        gameObject.GetComponentInChildren<CursorMovement>().WhichPlayer = Manager.instance.NbOfPlayer;
        if (Manager.instance.NbOfPlayer == 1)
        {
            gameObject.tag = "P1";
            MainSprite.color = Color.cyan;
            transform.position = Manager.instance.SpawnPoints[0].position;
            Manager.instance.NbOfPlayer++;
        }
        else
        {
            gameObject.tag = "P2";
            MainSprite.color = Color.red;
            transform.position = Manager.instance.SpawnPoints[1].position;
        }
    }

    void Update()
    {
        if (movementInput != Vector2.zero)
            Movement();
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    void Movement()
    {
        //Vector2 m = new Vector2(movementInput.x, movementInput.y) * Speed * Time.deltaTime;
        Vector2 m2 = new Vector2(movementInput.x, movementInput.y) * Speed;
        if (!CanMove)
            m2 = Vector2.zero;
        //transform.Translate(m, Space.World);
        rb.velocity = m2;
    }
}
