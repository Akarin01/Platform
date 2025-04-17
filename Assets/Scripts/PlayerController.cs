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

    // ��ͬ���ѡ��ͬ���ٶ�
    public float CurrentMoveSpeed
    {
        get
        {
            // ֹͣ���� ��ǽ ����ʱ�޷��ƶ� ����
            if (!IsMoving || physicsDetection.IsOnWall || !CanMove || !IsAlive)
            {
                return 0;
            }
            // ����
            if (!physicsDetection.IsGrounded)
            {
                return airWalkSpeed;
            }
            // ����
            if (!IsRunning)
            {
                return walkSpeed;
            }
            // �ܶ�
            return runSpeed;
        }
    }

    [Header("Statement")]
    [SerializeField] bool _isMoving;
    // �������Ե�ͬʱ����animator�Ĳ���
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
    // ����������ɷ�ת����ΪIsFacingRight�����ﳯ���ǰ󶨵�
    public bool IsFacingRight
    {
        get
        {
            return _isFacingRight;
        }
        private set
        {
            // ������ﳯ�����仯
            if (IsFacingRight != value)
            {
                // ��ת
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
        // �����ٶȣ����ٶ��ٶȸ�ֵ
        if (!damageable.LockVelocity)
        {
            // ����ˮƽ�ٶ�
            rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);

            animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);
        }


        #region ���ô�ֱ�ٶ�
        // �����׶�
        if (rb.velocity.y > 0 && !isJumping)
        {
            rb.velocity += Physics2D.gravity * (shortJumpGravityFactor - 1) * Time.fixedDeltaTime;
        }
        else if (rb.velocity.y > 0)
        {
            rb.velocity += Physics2D.gravity * (riseGravityFactor - 1) * Time.fixedDeltaTime;
        }
        // ����׶�
        else if (rb.velocity.y < 0)
        {
            rb.velocity += Physics2D.gravity * (fallGravityFactor - 1) * Time.fixedDeltaTime;
            // �����������ٶ�
            if (rb.velocity.y < -maxFallSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, -maxFallSpeed);
            }
        }
        #endregion
    }

    // ͨ��PlayerInput������¼�֪ͨ�÷�������ȡ����
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        IsMoving = moveInput.x != 0;
        if (IsAlive)
        {
            // ���������������ﳯ��
            SetFacingDirection(moveInput);
        }
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        // �������ƶ��������ﳯ��
        if (moveInput.x > 0 && !IsFacingRight)
        {
            IsFacingRight = true;
        }
        // �������ƶ��������ﳯ��
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
