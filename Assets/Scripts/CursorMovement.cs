using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CursorMovement : MonoBehaviour
{
    Vector2 movementInputRotate = Vector2.zero;
    Vector3 transferPosition;
    bool shoot, isCooldown;
    float nextAttack = 1f;

    public float attackRate = 1f;
    public GameObject Bullet;

    void Update()
    {
        if (movementInputRotate != Vector2.zero)
            Rotate();

        if (isCooldown == false && shoot)
        {
            isCooldown = true;
            nextAttack = attackRate;
            Shoot();
        }

        if (isCooldown)
        {
            nextAttack -= Time.deltaTime;
            if (nextAttack <= 0)
            {
                isCooldown = false;
            }
        }
    }

    public void OnRotate(InputAction.CallbackContext context)
    {
        movementInputRotate = context.ReadValue<Vector2>();
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        shoot = context.action.triggered;
        shoot = context.ReadValue<bool>();
    }

    void Rotate()
    {
        float angle = Mathf.Atan2(movementInputRotate.y, movementInputRotate.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        transferPosition = new Vector3(movementInputRotate.x + transform.position.x, movementInputRotate.y + transform.position.y, 0);
    }
    void Shoot()
    {
        print("shoot");
        Instantiate(Bullet, transferPosition, transform.rotation);
    }
}
