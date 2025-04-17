using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage = 10;
    public Vector2 velocity = new Vector2(7f, 0);
    public Vector2 hitForce = new Vector2(3f, 0);
    public float distanceToDestroy;
    Rigidbody2D rb;

    Vector3 originalPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        originalPosition = transform.position;

        int direction = transform.localScale.x > 0 ? 1 : -1;
        rb.velocity = new Vector2(velocity.x * direction, velocity.y);
    }

    private void Update()
    {
        if (Vector3.Distance(originalPosition, transform.position) >= distanceToDestroy)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();

        if (damageable != null)
        {
            Vector2 knockBack = transform.localScale.x > 0 ? hitForce : new Vector2(-hitForce.x, hitForce.y);
            damageable.Hit(damage, knockBack);
        }

        Destroy(gameObject);
    }
}
