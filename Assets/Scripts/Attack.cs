using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int attackDamage;
    public Vector2 hitForce;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();
        if (damageable != null)
        {
            Vector2 knockBack = transform.parent.localScale.x > 0 ? hitForce : new Vector2(-hitForce.x, hitForce.y);
            damageable.Hit(attackDamage, knockBack);
        }
    }
}
