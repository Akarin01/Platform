using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DetectionZone : MonoBehaviour
{
    public List<Collider2D> clds = new List<Collider2D>();
    Collider2D cld;

    public UnityEvent OnHaveNoneCollider;

    private void Awake()
    {
        cld = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (clds.Count == 0)
        {
            OnHaveNoneCollider?.Invoke();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        clds.Add(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        clds.Remove(collision);
    }
}
