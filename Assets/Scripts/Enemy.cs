using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Projectile;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour,IHittable
{
    private Rigidbody2D rb;

    private Transform player;

    [SerializeField] private SpriteRenderer spriteRend;
    [SerializeField]
    private bool active = false;

    [SerializeField]
    private Transform healthBar;


    [SerializeField]
    private float maxHealth;
    private float health;

    [Header("Effect Sprites")]
    [SerializeField] private Sprite polymorphSprite;
    [SerializeField] private Sprite freezeSprite;
    private Sprite origSprite;

    [Header("AI Behavior")]
    [SerializeField]
    private float speed;
    private float originalSpeed;
    [SerializeField]
    private float stoppingDistance;
    [SerializeField]
    private float retreatDistance;

    private enum EnemyType { Ranged, Melee }

    [SerializeField]
    private EnemyType enemyType = EnemyType.Ranged;

    private bool isFrozen;

    [Header("shooting")]
    [SerializeField]
    private GameObject projectileObject;
    [SerializeField]
    private Transform fireOffset;
    [SerializeField]
    private Transform firePoint;
    [SerializeField]
    private float startTimeBtwShots;
    private float timeBtwShots;



    public void Activate(Transform player)
    {
        this.player = player;
        active = true;
    }
    public void DoDamage(float damage)
    {
        if(active)
             health -= damage;

        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        float percent = health/maxHealth;
        healthBar.localScale = new Vector3(percent, healthBar.localScale.y, healthBar.localScale.z);
        
    }

    public void Hit(int dam, Projectile.Spell spellEffect)
    {
        DoDamage(dam);
        DoSpellEffect(spellEffect);
        Debug.Log(speed);
    }

    private void DoSpellEffect(Projectile.Spell spellEffect)
    {
        if(spellEffect == Projectile.Spell.Freeze)
        {
            speed = 0;
            spriteRend.sprite = freezeSprite;
            isFrozen = true;
            StartCoroutine(ThawOut(1f));
        }else if(spellEffect == Projectile.Spell.Explosion)
        {

        }else if(spellEffect == Projectile.Spell.PolyMorph)
        {
            StartCoroutine(UnPolyMorph(3f));
        }
    }

    IEnumerator UnPolyMorph(float time)
    {
        isFrozen = true;
        speed /= 4;
        spriteRend.sprite = polymorphSprite;
        yield return new WaitForSeconds(time);
        speed = originalSpeed;
        spriteRend.sprite = origSprite;
        isFrozen = false;
    }

    IEnumerator ThawOut(float time)
    {
        yield return new WaitForSeconds(time);

        speed = originalSpeed;
        spriteRend.sprite = origSprite;
        isFrozen = false;
    }


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.freezeRotation = true;

        spriteRend = GetComponentInChildren<SpriteRenderer>();
        health = maxHealth;
    }

    private void Start()
    {
        timeBtwShots = startTimeBtwShots;
        originalSpeed = speed;
        origSprite = spriteRend.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        if (!active)
            return;



        if (health <= 0)
            Destroy(gameObject);

        if (isFrozen) return;

        if (speed != 0)
            Movement();
        if (enemyType == EnemyType.Ranged)
        {
            RotateFirePoint();
            FireBullet();

        }
        else if (enemyType == EnemyType.Melee)
            Melee();

        UpdateAnimator();
    }

    private void Movement()
    {
        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);

        if (distanceFromPlayer > stoppingDistance)
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        else if (distanceFromPlayer < retreatDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);

        }
    }

    private void Melee()
    {
        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);
        if (distanceFromPlayer <= stoppingDistance && timeBtwShots <= 0)
        {
            player.GetComponent<Player>().Hit(1, Spell.Wound);
            timeBtwShots = startTimeBtwShots;
        }
        else
        {
            timeBtwShots -= Time.deltaTime;
        }
    }

    private void RotateFirePoint()
    {
        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);
        // Vector2 direction = (player.position - transform.position).normalized;
        if(distanceFromPlayer > retreatDistance)
        {
            float angle = Mathf.Atan2(player.position.y - fireOffset.position.y, player.position.x - fireOffset.position.x) * Mathf.Rad2Deg -90f;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            fireOffset.rotation = Quaternion.RotateTowards(fireOffset.rotation, targetRotation, 300f * Time.deltaTime);
        }
    }

    private void FireBullet()
    {
        if (timeBtwShots <= 0)
        {
            GameObject proj = Instantiate(projectileObject, firePoint.position, fireOffset.rotation);


            timeBtwShots = startTimeBtwShots;
        }
        else
            timeBtwShots -= Time.deltaTime;
    }

    [Header("Effects")]
    [SerializeField] private float aniMoveSpeed, yAmp = 0.1f, yFrq = 24f;
    void UpdateAnimator()
    {
        var direction = Mathf.Sign(player.transform.position.x-transform.position.x);
        spriteRend.transform.localScale = new Vector3(direction, 1f, 1f);

        Vector2 latSpeed = rb.velocity;
        aniMoveSpeed = Vector3.SqrMagnitude(latSpeed);
        if (aniMoveSpeed > 0)
        {
            float yPos = Mathf.Sin(Time.time * yFrq) * yAmp;
            spriteRend.gameObject.transform.localPosition = new Vector3(0, yPos, 0);//bounce sprite
        }
        else
            spriteRend.gameObject.transform.localPosition = Vector3.zero;
    }
}
