using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(PhysicsDetection))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed;
    public float runSpeed;
    public float airWalkSpeed;
    [Header("Jump")]
    public float jumpImpulse;
    public float riseGravityFactor;
    public float shortJumpGravityFactor;
    public float fallGravityFactor;
    public float maxFallSpeed;
    bool isJumping;

    Vector2 moveInput;

    // 不同情况选择不同的速度
    public float CurrentMoveSpeed
    {
        get
        {
            // 停止输入 碰墙 攻击时无法移动 死亡
            if (!IsMoving || physicsDetection.IsOnWall || !CanMove || !IsAlive)
            {
                return 0;
            }
            // 空中
            if (!physicsDetection.IsGrounded)
            {
                return airWalkSpeed;
            }
            // 行走
            if (!IsRunning)
            {
                return walkSpeed;
            }
            // 跑动
            return runSpeed;
        }
    }

    [Header("Statement")]
    [SerializeField] bool _isMoving;
    // 设置属性的同时设置animator的参数
    public bool IsMoving
    {
        get
        {
            return _isMoving;
        }
        private set
        {
            _isMoving = value;
            animator.SetBool(AnimationStrings.isMoving, _isMoving);
        }
    }

    [SerializeField] bool _isRunning;
    public bool IsRunning
    {
        get
        {
            return _isRunning;
        }
        private set
        {
            _isRunning = value;
            animator.SetBool(AnimationStrings.isRunning, _isRunning);
        }
    }

    [SerializeField] bool _isFacingRight;
    // 利用属性完成翻转，因为IsFacingRight与人物朝向是绑定的
    public bool IsFacingRight
    {
        get
        {
            return _isFacingRight;
        }
        private set
        {
            // 如果人物朝向发生变化
            if (IsFacingRight != value)
            {
                // 翻转
                transform.localScale *= new Vector2(-1, 1);
            }
            _isFacingRight = value;
        }
    }

    public bool CanMove => animator.GetBool(AnimationStrings.canMove);
    public bool IsAlive => animator.GetBool(AnimationStrings.isAlive);

    Rigidbody2D rb;
    Animator animator;
    PhysicsDetection physicsDetection;
    Damageable damageable;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        physicsDetection = GetComponent<PhysicsDetection>();
        damageable = GetComponent<Damageable>();
    }

    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        // 锁定速度，不再对速度赋值
        if (!damageable.LockVelocity)
        {
            // 设置水平速度
            rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);

            animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);
        }


        #region 设置垂直速度
        // 上升阶段
        if (rb.velocity.y > 0 && !isJumping)
        {
            rb.velocity += Physics2D.gravity * (shortJumpGravityFactor - 1) * Time.fixedDeltaTime;
        }
        else if (rb.velocity.y > 0)
        {
            rb.velocity += Physics2D.gravity * (riseGravityFactor - 1) * Time.fixedDeltaTime;
        }
        // 下落阶段
        else if (rb.velocity.y < 0)
        {
            rb.velocity += Physics2D.gravity * (fallGravityFactor - 1) * Time.fixedDeltaTime;
            // 限制最大掉落速度
            if (rb.velocity.y < -maxFallSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, -maxFallSpeed);
            }
        }
        #endregion
    }

    // 通过PlayerInput组件的事件通知该方法来获取输入
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        IsMoving = moveInput.x != 0;
        if (IsAlive)
        {
            // 根据输入设置人物朝向
            SetFacingDirection(moveInput);
        }
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        // 当向右移动并且人物朝左
        if (moveInput.x > 0 && !IsFacingRight)
        {
            IsFacingRight = true;
        }
        // 当向左移动并且人物朝右
        else if (moveInput.x < 0 && IsFacingRight)
        {
            IsFacingRight = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && physicsDetection.IsGrounded && CanMove)
        {
            animator.SetTrigger(AnimationStrings.jumpTrigger);
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
            isJumping = true;
        }
        if (context.canceled)
        {
            isJumping = false;
        }
    }

    public void OnRangedAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationStrings.rangedAttackTrigger);
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsRunning = true;
        }
        else if (context.canceled)
        {
            IsRunning = false;
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationStrings.attackTrigger);
        }
    }

    public void OnHit(Vector2 hitForce)
    {
        rb.velocity = new Vector2(hitForce.x, rb.velocity.y + hitForce.y);
    }

}
