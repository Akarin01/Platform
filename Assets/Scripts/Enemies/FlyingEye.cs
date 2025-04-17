using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEye : MonoBehaviour
{
    public float flySpeed = 3f;
    public float distanceEpsilon;
    public List<Transform> wayPoints;
    int targetIndex = 0;

    [SerializeField] bool _hasTarget;
    public bool HasTarget
    {
        get
        {
            return _hasTarget;
        }

        set
        {
            _hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
        }
    }

    public bool CanAttack
    {
        get
        {
            return animator.GetBool(AnimationStrings.canAttack);
        }
        set
        {
            animator.SetBool(AnimationStrings.canAttack, value);
        }
    }

    public bool CanMove => animator.GetBool(AnimationStrings.canMove);

    public float attackInterval;
    float attackTimer;

    public DetectionZone detectionZone;
    public Collider2D deathCollider;
    Rigidbody2D rb;
    Animator animator;
    Damageable damageable;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();
    }

    private void Update()
    {
        HasTarget = detectionZone.clds.Count > 0;

        if (!CanAttack)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer > attackInterval)
            {
                CanAttack = true;
                attackTimer -= attackInterval;
            }
        }
    }

    private void FixedUpdate()
    {
        if (damageable.IsAlive)
        {
            if (CanMove)
            {
                Flight();
            }
            else
            {
                rb.velocity = Vector2.zero;
            }
        }
    }

    private void Flight()
    {
        Transform nextPoint = wayPoints[targetIndex];
        Vector2 direction = (nextPoint.position - transform.position).normalized;

        UpdateFacingDirection();

        rb.velocity = direction * flySpeed;

        float distance = Vector2.Distance(transform.position, nextPoint.position);
        // 到达目标点
        if (distance <= distanceEpsilon)
        {
            targetIndex++;
            if (targetIndex >= wayPoints.Count)
            {
                targetIndex = 0;
            }
        }
    }

    private void UpdateFacingDirection()
    {
        Vector3 originalScale = transform.localScale;
        if (originalScale.x < 0 && rb.velocity.x > 0)
        {
            transform.localScale = new Vector3(originalScale.x * -1, originalScale.y, originalScale.z);
        }
        else if(originalScale.x > 0 && rb.velocity.x < 0)
        {
            transform.localScale = new Vector3(originalScale.x * -1, originalScale.y, originalScale.z);
        }
    }

    public void EndAttack()
    {
        CanAttack = false;
    }

    public void OnDeath()
    {
        // 掉落
        rb.velocity = new Vector2(0, rb.velocity.y);
        rb.gravityScale = 2f;

        deathCollider.enabled = true;
    }
}
