using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsDetection : MonoBehaviour
{
    CapsuleCollider2D capsuleCld;
    Animator animator;
    public ContactFilter2D castFilter;
    RaycastHit2D[] groundHits = new RaycastHit2D[5];
    RaycastHit2D[] wallHits = new RaycastHit2D[5];
    RaycastHit2D[] ceilingHits = new RaycastHit2D[5];
    public float groundDistance;
    public float wallDistance;
    public float ceilingDistance;
    Vector2 wallDetectDirection;


    [SerializeField] bool _isGrounded;
    public bool IsGrounded
    {
        get
        {
            return _isGrounded;
        }
        private set
        {
            _isGrounded = value;
            animator.SetBool(AnimationStrings.isGrounded, value);
        }
    }

    [SerializeField] bool _isOnWall;
    public bool IsOnWall
    {
        get
        {
            return _isOnWall;
        }
        private set
        {
            _isOnWall = value;
            animator.SetBool(AnimationStrings.isOnWall, value);
        }
    }


    [SerializeField] bool _isOnCeiling;
    public bool IsOnCeiling
    {
        get
        {
            return _isOnCeiling;
        }
        private set
        {
            _isOnCeiling = value;
            animator.SetBool(AnimationStrings.isOnCeiling, value);
        }
    }

    private void Awake()
    {
        capsuleCld = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        wallDetectDirection = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

        IsGrounded = capsuleCld.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0;
        IsOnWall = capsuleCld.Cast(wallDetectDirection, castFilter, wallHits, wallDistance) > 0;
        IsOnCeiling = capsuleCld.Cast(Vector2.up, castFilter, ceilingHits, ceilingDistance) > 0;
    }
}
