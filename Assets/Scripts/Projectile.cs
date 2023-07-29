using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private int damage;
    private int lifeTime = 3;
    public float speed = 10f;
    private float bounceForce = 0.8f;

    [SerializeField]
    private int pierceNumber = 2;

    [SerializeField]
    private int damageTickMax = 2;


    private Vector2 direction;

    private bool canHitSelf;
    private GameObject owner;
    private Rigidbody2D rb;


    public enum Spell { Wound, Skewer, Guardian, Freeze, Explosion, PolyMorph }
    [SerializeField]
    private Spell spellEffect;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        direction = transform.up;
        rb.velocity = direction * speed;
    }
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!canHitSelf && other.gameObject == owner)
            return;

        IHittable hit = other.GetComponent<IHittable>();
        if (hit != null)
        {
            hit.Hit(damage, spellEffect);
            if(spellEffect == Spell.Skewer && pierceNumber  > 0)
            {
                pierceNumber--;
            }
            else if(spellEffect == Spell.Wound)
            {
                StartCoroutine(WoundDamage(hit, 1f));   
            }else
                Destroy(gameObject);
        }

    }


    private IEnumerator WoundDamage(IHittable enemy,float time)
    {
        while(damageTickMax > 0)
        {
            yield return new WaitForSeconds(time);
            damageTickMax--;
            enemy.Hit(damage, Spell.Wound);

        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject != owner)
        {
            Vector2 normal = collision.GetContact(0).normal;
            Vector2 reflectedDirection = Vector2.Reflect(direction, normal);

            direction = reflectedDirection.normalized;

            lifeTime -= 1;

            if (lifeTime <= 0)
            {
                Destroy(gameObject);
            }

            rb.velocity = direction * speed * Mathf.Pow(bounceForce, 4 - lifeTime);

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle-90f));
        }
    }


}
