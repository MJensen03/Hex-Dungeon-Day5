using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour,IHittable
{
    Rigidbody2D rb;
    Animator animator;

    RestartButtonScript deathUI;

    [Header("Stats")]
    [SerializeField]
    private int maxHealth;
    private int curHealth;
    HPUIScript healthUI;
    [SerializeField] GameObject deathAnim;

    [Header("Movement")]
    [SerializeField]
    private float moveSpeed;
    Vector2 moveAxis,lookAxis;
    [SerializeField] private PlayerControls controls;
    private InputAction move, look, fire;

    [Header("Shooting")]
    GameObject gunUIObj;
    [SerializeField] GameObject[] projectileObjects;
    [SerializeField] private Transform weapon,firePoint;
    [SerializeField] private float rotSpeed = 8f;
    [Header("Ammo")]
    public List<int> gunChamber = new List<int> { 2, 2, 2, 2, 2, 5 };
    BulletsUIScript bulletsUI;

    [Header("Effects")]
    [SerializeField] private SpriteRenderer spriteRend;
    private Material defaultMat, flashMat;
    [SerializeField] private float yAmp = 0.1f, yFrq = 16f;
    [SerializeField] private float aniMoveSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        gunUIObj = GameObject.Find("Gun");
        deathUI = GameObject.Find("DeathScreenUI").GetComponent<RestartButtonScript>();
        animator = transform.Find("PlayerSprite").GetComponent<Animator>();
        bulletsUI = gunUIObj.GetComponent<BulletsUIScript>();
        healthUI = gunUIObj.GetComponent<HPUIScript>();
        rb.gravityScale = 0;
        rb.freezeRotation = true;

        curHealth = maxHealth;
        controls = new PlayerControls();
    }
    private void OnEnable()
    {
        move = controls.Player.Move;
        move.Enable();

        fire = controls.Player.Fire;
        fire.Enable();
        fire.performed += Fire;
    }
    private void OnDisable()
    {
        move.Disable();
        fire.Disable();
    }
    private void Start()
    {
        healthUI.SetHealth(curHealth);
        bulletsUI.SetBullets(gunChamber.Count);
        Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera.Follow = this.gameObject.transform;
    }
    // Update is called once per frame
    void Update()
    {
        moveAxis = move.ReadValue<Vector2>();
        if (moveAxis != Vector2.zero)
        {
            RotateWeapon();
        }
        UpdateAnimator();
    }

    void Death()
    {
        Instantiate(deathAnim, transform.position, transform.rotation);
        animator.SetTrigger("Death");
        OnDisable();
        GameObject.Find("GunOffset").SetActive(false);
        gunUIObj.SetActive(false);
        deathUI.InitiateUI();
    }


    public void Reload(int min,int max)
    {
        gunChamber.Clear();
        for (int i = 0; i <= 5; i++)
        {
            gunChamber.Add(Random.Range(min, max));
        }
        bulletsUI.SetBullets(gunChamber.Count);
        firePoint.DOPunchRotation(new Vector3(0, 0, 361), .25f);
    }
    private void RotateWeapon()
    {
        lookAxis = new Vector2(Mathf.Sin(moveAxis.x), Mathf.Sin(moveAxis.y));
        float angle = Mathf.Atan2(lookAxis.y, lookAxis.x) * Mathf.Rad2Deg;
        Quaternion newRot = Quaternion.Euler(0, 0, angle - 90f);
        weapon.rotation = Quaternion.Slerp(transform.rotation, newRot, rotSpeed);
    }

    void UpdateAnimator()
    {
        Vector2 latSpeed = rb.velocity;
        aniMoveSpeed = Vector3.SqrMagnitude(latSpeed);
        if (aniMoveSpeed>0)
        {
            float yPos = Mathf.Sin(Time.time * yFrq) * yAmp;
            spriteRend.gameObject.transform.localPosition = new Vector3(0, yPos, 0);
        }
        else
            spriteRend.gameObject.transform.localPosition = Vector3.zero;

        var direction = Mathf.Sign(lookAxis.x);
        spriteRend.transform.localScale = new Vector3(direction, 1f, 1f);
        firePoint.transform.localScale = new Vector3(direction, 1f, 1f);

    }
    private void Fire(InputAction.CallbackContext context)
    {
        if (gunChamber.Count == 0)//reload
        {
            Reload(1, 6);
        }
        else//fire
        {
            GameObject proj = Instantiate(projectileObjects[gunChamber[0]], firePoint.transform.position, weapon.rotation);
            gunChamber.RemoveAt(0);
            weapon.DOPunchRotation(new Vector3(0, 0, 60f), 0.12f);

            bulletsUI.SetBullets(gunChamber.Count);
        }
    }
    public void DoDamage(int damage)
    {
        curHealth -= damage;
        healthUI.SetHealth(curHealth);

        if (curHealth <= 0)
        {
            Death();
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = moveAxis.normalized * moveSpeed /** Time.fixedDeltaTime*/;
        //rb.MovePosition(rb.position + moveAxis.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    public void Hit(int dam)
    {
        DoDamage(dam);
    }
    
}
