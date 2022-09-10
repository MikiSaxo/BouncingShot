using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class CursorMovement : MonoBehaviour
{
    Vector2 movementInputRotate = Vector2.zero;
    Vector3 transferPosition;
    bool shoot, isCooldown;
    public bool IsLock;
    float nextAttack = 1f;

    public float attackRate = 1f;
    public int WhichPlayer;
    [SerializeField] GameObject Bullet, Cursor, SpawnBullet;
    [SerializeField] GameObject[] IndicatorsCanShoot;
    [SerializeField] float timer;

    const float minimumTime = 0.05f;

    private void Start()
    {
        IsLock = true;
        StartCoroutine(DeLock());
    }

    void Update()
    {
        if (IsLock)
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
            IndicatorsCanShoot[0].SetActive(false);
            IndicatorsCanShoot[1].SetActive(false);
            isCooldown = true;
            nextAttack = attackRate;
            Shoot();
        }

        if (isCooldown)
        {
            nextAttack -= Time.deltaTime;
            if (nextAttack <= attackRate/2)
                IndicatorsCanShoot[0].SetActive(true);
                IndicatorsCanShoot[0].GetComponent<ReboundAnimation>().StartBounce();

            if (nextAttack <= 0)
            {
                isCooldown = false;
                IndicatorsCanShoot[1].SetActive(true);
                IndicatorsCanShoot[1].GetComponent<ReboundAnimation>().StartBounce();
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
    }

    public void OnLock(InputAction.CallbackContext context)
    {
        IsLock = context.action.triggered;
        print("lockkkkkk");
    }

     
    void Rotate()
    {
        if (!IsLock)
        {
            float angle = Mathf.Atan2(movementInputRotate.y, movementInputRotate.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }

    void RotateLock()
    {
        Vector2 direction = Manager.instance.Ball.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 50 * Time.deltaTime);
    }

    IEnumerator DeLock()
    {
        yield return new WaitForSeconds(1f);
        IsLock = false;
    }

    void Shoot()
    {
        gameObject.GetComponent<ReboundAnimation>().StartBounce();
        print("shoot");
        transferPosition = new Vector3(SpawnBullet.transform.position.x, SpawnBullet.transform.position.y, 0);
        GameObject b = Instantiate(Bullet, transferPosition, Cursor.transform.rotation);
        
        if (GetComponent<WhoAreYou>().ChoisiBieng == WhoAreYou.ChooseYourChampion.P2)
            b.gameObject.GetComponent<WhoAreYou>().ChoisiBieng = WhoAreYou.ChooseYourChampion.BulletP2;
        
        if (WhichPlayer == 1)
        {
            b.GetComponentInChildren<SpriteRenderer>().color = Manager.instance.statesColor[1];
            b.GetComponentInChildren<TrailRenderer>().startColor = Manager.instance.statesColor[1];
        }
        else if (WhichPlayer == 2)
        {
            b.GetComponentInChildren<SpriteRenderer>().color = Manager.instance.statesColor[2];
            b.GetComponentInChildren<TrailRenderer>().startColor = Manager.instance.statesColor[2];
        }
    }
}