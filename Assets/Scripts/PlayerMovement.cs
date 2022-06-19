using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float Speed;

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
        transform.Translate(m, Space.World);
    }
}
