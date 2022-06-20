using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float Speed;
    public Rigidbody2D rb;
    public SpriteRenderer MainSprite;

    Vector2 movementInput = Vector2.zero;

    private void Start()
    {
        print(Manager.instance.NbOfPlayer);
        if (Manager.instance.NbOfPlayer == 1)
        {
            MainSprite.color = Color.blue;
            transform.position = Manager.instance.SpawnPoints[0].position;
            Manager.instance.NbOfPlayer++;
        }
        else
        {
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
        Vector2 m = new Vector2(movementInput.x, movementInput.y) * Speed * Time.deltaTime;
        Vector2 m2 = new Vector2(movementInput.x, movementInput.y) * Speed;
        //transform.Translate(m, Space.World);
        rb.velocity = m2;
    }
}
