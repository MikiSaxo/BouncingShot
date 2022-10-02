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
    //[SerializeField] GameObject canShootDetector;
    [SerializeField] GameObject normalScaleSprite;
    [SerializeField] GameObject fx_Shoot;
    [SerializeField] Transform fx_Spawn;
    private Vector3 getNormalSpritePos;
    private bool canShoot = false;
    private float facingAngle = 0;
    private bool hasLock = false;

    [SerializeField] float[] vib_Shoot;
    [SerializeField] float[] vib_Look;

    private void Start()
    {
        IsLock = true;
        canShoot = true;
        StartCoroutine(DeLock());
    }

    void Update()
    {
        if (IsLock)
            RotateLock();
        else
        {
            hasLock = false;
            if (movementInputRotate != Vector2.zero)
                Rotate();
        }

        if (isCooldown == false && shoot && gameObject.GetComponent<PlayerMovement>().CanMove && canShoot)
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
            if (nextAttack <= attackRate / 2)
                IndicatorsCanShoot[0].SetActive(true);
            IndicatorsCanShoot[0].GetComponent<ReboundAnimation>().StartBounce();

            if (nextAttack <= 0)
            {
                isCooldown = false;
                IndicatorsCanShoot[1].SetActive(true);
                IndicatorsCanShoot[1].GetComponent<ReboundAnimation>().StartBounce();
            }
        }

        //if (normalScaleSprite)
        //{
        normalScaleSprite.transform.position = Vector3.Lerp(normalScaleSprite.transform.position, transform.position, Time.deltaTime * 5);
        //}
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
    }

    public void CanShoot(bool which)
    {
        canShoot = which;
    }

    void Rotate()
    {
        if (!IsLock)
        {
            float angle = Mathf.Atan2(movementInputRotate.y, movementInputRotate.x) * Mathf.Rad2Deg;
            facingAngle = angle;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }

    void RotateLock()
    {
        if (!hasLock)
        {
            gameObject.GetComponent<VibrateController>().StartVibration(vib_Look[0], vib_Look[1], vib_Look[2]);
            hasLock = true;
        }

        Vector2 direction = Manager.instance.Ball.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        facingAngle = angle;
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
        AudioManager.Instance.PlaySound("PlayerShoot");

        gameObject.GetComponent<ReboundAnimation>().StartBounce();
        gameObject.GetComponent<VibrateController>().StartVibration(vib_Shoot[0], vib_Shoot[1], vib_Shoot[2]);

        MoveBackfeedback();
        transferPosition = new Vector3(SpawnBullet.transform.position.x, SpawnBullet.transform.position.y, 0);
        GameObject b = Instantiate(Bullet, transferPosition, Cursor.transform.rotation);
        GameObject go = Instantiate(fx_Shoot, transferPosition, Cursor.transform.rotation);
        go.transform.position = fx_Spawn.position;

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
    const float _powerBack = -.3f;
    void MoveBackfeedback()
    {
        getNormalSpritePos = normalScaleSprite.transform.position;
        
        Vector2 finalMovement = new Vector2(Mathf.Cos(facingAngle * Mathf.Deg2Rad), Mathf.Sin(facingAngle * Mathf.Deg2Rad));
        normalScaleSprite.transform.DOMove(new Vector3(getNormalSpritePos.x + finalMovement.x * _powerBack, getNormalSpritePos.y + finalMovement.y * _powerBack, 0), 0f);
    }
}