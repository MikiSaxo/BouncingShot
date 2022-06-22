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
    public GameObject Bullet, BBullet;
    public GameObject Cursor;

    //[SerializeField] GameObject b;
    [SerializeField] float timer;

    private void Start()
    {
        timer = 0;
    }

    void Update()
    {
        Rotate();
        Timer();

        //if (isCooldown == false && !shoot && timer > 0)
        //{
        //    isCooldown = true;
        //    nextAttack = attackRate;
        //    //Shoot();
        //}

        //if (isCooldown)
        //{
        //    nextAttack -= Time.deltaTime;
        //    if (nextAttack <= 0)
        //    {
        //        isCooldown = false;
        //    }
        //}
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

    void Timer()
    {
        if (shoot == true)
        {
            if (timer <= 1)
                timer += Time.deltaTime;
        }
        else
        {
            if (gameObject.GetComponent<PlayerMovement>().CanMove)
            {
                if (timer > 0.05f)
                {
                    Shoot();
                    //timer = 0;
                }
            }
        }
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
        b.transform.localScale = new Vector3(1 * timer, 1 * timer, 1);
        b.GetComponent<Bullet>().timer = timer;
        timer = 0;
        if (WhichPlayer == 1)
        {
            b.tag = "BulletP1";
            b.GetComponentInChildren<SpriteRenderer>().color = Color.cyan;
        }
        else if (WhichPlayer == 2)
        {
            b.tag = "BulletP2";
            b.GetComponentInChildren<SpriteRenderer>().color = Color.red;
        }
        //timer = 0;
    }
}