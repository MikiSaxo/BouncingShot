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
    public int WhichPlayer;
    public GameObject Bullet;
    public GameObject Cursor;

    void Update()
    {
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
        Cursor.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        
        transferPosition = new Vector3(movementInputRotate.x + Cursor.transform.position.x, movementInputRotate.y + Cursor.transform.position.y, 0);
    }
    void Shoot()
    {
        print("shoot");
        GameObject b = Instantiate(Bullet, transferPosition, Cursor.transform.rotation);
        if (WhichPlayer == 1)
        {
            b.tag = "BulletP1";
            b.GetComponent<SpriteRenderer>().color = Color.cyan;
        }
        else if (WhichPlayer == 2)
        {
            b.tag = "BulletP2"; 
            b.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }
}
