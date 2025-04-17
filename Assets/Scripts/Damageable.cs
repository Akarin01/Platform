using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    public UnityEvent<Vector2> OnHit;
    public UnityEvent OnDeath;
    public UnityEvent OnHealthChange;
    public int maxHealth;

    // �����ܻ�����ʱ�����ٶȣ���ֹ���˹������������ٶȶ����»���ʧЧ
    public bool LockVelocity
    {
        get
        {
            return animator.GetBool(AnimationStrings.lockVelocity);
        }
    }

    [SerializeField] int _currentHealth;
    // ����ֵ����ʱ�Զ�����isAlive
    public int CurrentHealth
    {
        get
        {
            return _currentHealth;
        }
        set 
        { 
            _currentHealth = value;
            OnHealthChange?.Invoke();
            if(value <= 0)
            {
                IsAlive = false;
                _currentHealth = 0;
            }
        }
    }

    [SerializeField] bool _isAlive = true;
    public bool IsAlive
    {
        get
        {
            return _isAlive;
        }
        set
        {
            _isAlive = value;
            animator.SetBool(AnimationStrings.isAlive, value);
            if (value == false)
            {
                OnDeath?.Invoke();
            }
        }
    }

    public bool isInvincible;
    public float invincibilityTime;
    float timerInvincibility;

    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        CurrentHealth = maxHealth;
    }

    private void Update()
    {
        // �޵�ʱ�䵹��ʱ
        if (isInvincible)
        {
            timerInvincibility += Time.deltaTime;
            if (timerInvincibility >= invincibilityTime)
            {
                timerInvincibility = 0;
                isInvincible = false;
            }
        }
    }

    public void Hit(int damage, Vector2 hitForce)
    {
        if (IsAlive && !isInvincible)
        {
            // �۳�����ֵ
            CurrentHealth -= damage;
            // �����ܻ�״̬
            animator.SetTrigger(AnimationStrings.hit);
            // ����Ч��
            OnHit?.Invoke(hitForce);
            
            CharacterEvents.onCharacterDameged.Invoke(gameObject, damage);

            // �����޵�ʱ��
            isInvincible = true;
        }
    }

    public bool Heal(int healthRestore)
    {
        int actualHeal = Mathf.Min(healthRestore, maxHealth - CurrentHealth);
        if (IsAlive && actualHeal > 0)
        {
            CurrentHealth += actualHeal;
            CharacterEvents.onCharacterHealed.Invoke(gameObject, actualHeal);
            return true;
        }
        return false;
    }
}
