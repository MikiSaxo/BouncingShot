using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CursorMovement : MonoBehaviour
{
    Vector2 movementInputRotate = Vector2.zero;
    Vector3 transferPosition;
    bool shoot, isLock, isCooldown;
    float nextAttack = 1f;

    public float attackRate = 1f;
    public int WhichPlayer;
    [SerializeField] GameObject Bullet, Cursor, SpawnBullet, IndicatorCanShoot;
    [SerializeField] float timer;

    const float minimumTime = 0.05f;

    private void Start()
    {
        //timer = 0;
    }

    void Update()
    {
        //Timer();
        if (isLock)
        {
            RotateLock();
        }
        else
        {
            if (movementInputRotate != Vector2.zero)
                Rotate();
        }

        if (isCooldown == false && shoot && gameObject.GetComponent<PlayerMovement>().CanMove)
        {
            IndicatorCanShoot.SetActive(false);
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
                IndicatorCanShoot.SetActive(true);
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
        //shoot = context.ReadValue<bool>();
    }

    public void OnLock(InputAction.CallbackContext context)
    {
        isLock = context.action.triggered;
        //isLock = context.ReadValue<bool>();
        print("lockkkkkk");
    }

    //void Timer()
    //{
    //    if (shoot == true)
    //    {
    //        timer += Time.deltaTime;
    //    }
    //    else
    //    {
    //        if (gameObject.GetComponent<PlayerMovement>().CanMove && timer > minimumTime)
    //            Shoot();
    //    }
    //}

    void Rotate()
    {
        if (!isLock)
        {
            float angle = Mathf.Atan2(movementInputRotate.y, movementInputRotate.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }

    void RotateLock()
    {
        //print("rotalocks");
        Vector2 direction = Manager.instance.Ball.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 50 * Time.deltaTime);
    }

    void Shoot()
    {
        //if (timer > 1)
        //    timer = 1;
        //RipplePostProcessor.instance.RippleEffect(transform.position);
        print("shoot");
        transferPosition = new Vector3(SpawnBullet.transform.position.x, SpawnBullet.transform.position.y, 0);
        GameObject b = Instantiate(Bullet, transferPosition, Cursor.transform.rotation);
        //if (timer < 0.2f)
        //    timer = minimumTime*2;
        //b.transform.localScale = new Vector3(1 * timer, 1 * timer, 1);
        //b.GetComponent<Bullet>().timer = timer;
        //timer = 0;
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
    }
}