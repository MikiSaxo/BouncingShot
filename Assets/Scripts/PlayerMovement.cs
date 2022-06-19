using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float Speed;

    Vector2 movementInput = Vector2.zero;
    Vector2 movementInputRotate = Vector2.zero;

    void Update()
    {
        if (movementInput != Vector2.zero)
            Movement();
        if (movementInputRotate != Vector2.zero)
            Rotate();
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }
    public void OnRotate(InputAction.CallbackContext context)
    {
        movementInputRotate = context.ReadValue<Vector2>();
    }

    void Movement()
    {
        Vector2 m = new Vector2(movementInput.x, movementInput.y) * Speed * Time.deltaTime;
        transform.Translate(m, Space.World);
    }

    void Rotate()
    {
        float angle = Mathf.Atan2(movementInputRotate.y, movementInputRotate.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
