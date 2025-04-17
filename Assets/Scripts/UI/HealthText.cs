using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthText : MonoBehaviour
{
    public Vector3 moveVelocity = new Vector3(0, 75, 0);

    public float timeToFade = 1f;
    float fadeTimer = 0f;

    Color startColor;

    RectTransform rectTransform;
    TextMeshProUGUI textMeshPro;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        startColor = textMeshPro.color;
    }

    private void Update()
    {
        rectTransform.position += moveVelocity * Time.deltaTime;


        if (fadeTimer < timeToFade)
        {
            fadeTimer += Time.deltaTime;
            float newAlpha = startColor.a * (1 - fadeTimer / timeToFade);
            textMeshPro.color = new Color(startColor.r, startColor.g, startColor.b, newAlpha);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
