using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float Speed;
    public Rigidbody2D rb;
    Vector2 movementInput = Vector2.zero;

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
