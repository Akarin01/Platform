using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public AudioClip pickupSFX;
    [Range(0f, 1f)] public float sfxVolume;
    public int healthRestore = 20;
    public Vector3 rotationVelocity;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();
        if (damageable != null)
        {
            if (damageable.Heal(healthRestore))
            {
                SoundManager.Instance.PlaySound(pickupSFX, sfxVolume);
                Destroy(gameObject);
            }
        }
    }

    private void Update()
    {
        transform.eulerAngles += rotationVelocity * Time.deltaTime;
    }
}
