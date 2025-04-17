using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Knight : MonoBehaviour
{
    public float accelerate = 3f;
    public float walkSpeed = 3f;
    public float walkStopRate = 0.05f;

    public float attackInterval;
    float attackTimer;

    public enum WalkableDirection
    {
        Right,
        Left,
    }

    // 设置WalkDirection的同时完成翻转与确定walkDirection
    // 用WalkDirection封装更改localScale和更改walkDirectionVector
    WalkableDirection _walkDirection;
    public WalkableDirection WalkDirection
    {
        get
        {
            return _walkDirection;
        }
        set
        {
            if (_walkDirection != value)
            {
                // 翻转
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            }
            // 设置walkDirection
            if (value == WalkableDirection.Left)
            {
                walkDirectionVector = Vector2.left;
            }
            else if (value == WalkableDirection.Right)
            {
                walkDirectionVector = Vector2.right;
            }
            _walkDirection = value;
        }
    }

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
    public bool IsAlive =>animator.GetBool(AnimationStrings.isAlive);

    public DetectionZone attackZone;
    Rigidbody2D rb;
    PhysicsDetection physicsDetection;
    Animator animator;
    Damageable damageable;

    Vector2 walkDirectionVector = Vector2.right;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        physicsDetection = GetComponent<PhysicsDetection>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();
    }

    private void Update()
    {
        HasTarget = attackZone.clds.Count > 0;

        if (!CanAttack)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackInterval) 
            {
                attackTimer -= attackInterval;
                CanAttack = true;
            }
        }
    }

    private void FixedUpdate()
    {
        // 撞墙后转向
        // 放在fixedupdate中执行，因为physiscDetection是Physics检测
        // 如果放在update中执行，在两次Physics检测间隔中执行多次update会导致角色一直反转
        if (physicsDetection.IsOnWall)
        {
            FlipDirection();
        }

        if (!IsAlive)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        if (damageable.LockVelocity)
        {
            return;
        }

        if (CanMove && physicsDetection.IsGrounded)
        {
            float currentvelocity = Mathf.Clamp(rb.velocity.x + (walkDirectionVector.x * accelerate * Time.fixedDeltaTime), -walkSpeed, walkSpeed);
            rb.velocity = new Vector2(currentvelocity, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, walkStopRate), rb.velocity.y);
        }
        
    }

    public void FlipDirection()
    {
        if (!physicsDetection.IsGrounded)
        {
            return;
        }

        if (WalkDirection == WalkableDirection.Left)
        {
            WalkDirection = WalkableDirection.Right;
        }
        else if (WalkDirection == WalkableDirection.Right)
        {
            WalkDirection = WalkableDirection.Left;
        }
        else
        {
            Debug.LogError("WalkDirection被设置为无效的值");
        }
    }

    public void OnHit(Vector2 hitForce)
    {
        rb.velocity = new Vector2(hitForce.x, rb.velocity.y + hitForce.y);
        if (hitForce.x > 0)
        {
            WalkDirection = WalkableDirection.Left;
        }
        else if(hitForce.x < 0)
        {
            WalkDirection = WalkableDirection.Right;
        }
    }

    public void EndAttack()
    {
        CanAttack = false;
    }
}
