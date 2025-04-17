using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    Damageable damageable;
    public TMP_Text healthText;
    Slider healthBar;

    private void Awake()
    {
        healthBar = GetComponent<Slider>();
        damageable = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Damageable>();
        if (damageable == null)
        {
            Debug.Log("没有找到player物体");
        }
    }
    private void OnEnable()
    {
        damageable.OnHealthChange.AddListener(OnHealthChange);
    }

    private void OnDisable()
    {
        damageable.OnHealthChange.RemoveListener(OnHealthChange);
    }

    void OnHealthChange()
    {
        healthText.text = $"Health  {damageable.CurrentHealth} / {damageable.maxHealth}";
        healthBar.value = (float)damageable.CurrentHealth / damageable.maxHealth;
    }
}
