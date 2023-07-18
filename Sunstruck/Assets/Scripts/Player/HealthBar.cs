using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public float maxHealth = 100f;
    public float damageRate = 10f;
    public float regenRate = 5f; 

    private float currentHealth;
    private float lastDamageTime;
    public Gradient gradient;
    public Image fill;

    public Light2D playerLight;
    public float damageCooldown = 2f;

    private void Start()
    {

        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
        lastDamageTime = Time.time;
        fill.color = gradient.Evaluate(1f);
        SetActive(false);
    }

    private void Update()
    {
        if (Time.time - lastDamageTime >= damageCooldown && currentHealth < maxHealth)
        {
            currentHealth += regenRate * Time.deltaTime;
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
            healthSlider.value = currentHealth;
            fill.color = gradient.Evaluate(healthSlider.normalizedValue);
            UpdateLightIntensity();
        }
    }

    public void TakeDamage()
    {
        if (Time.time - lastDamageTime >= 1f)
        {
            currentHealth -= damageRate;
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
            healthSlider.value = currentHealth;
            lastDamageTime = Time.time;
            fill.color = gradient.Evaluate(healthSlider.normalizedValue);
            UpdateLightIntensity();
        }
    }

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    private void UpdateLightIntensity()
    {
        if (playerLight != null)
        {
            playerLight.intensity = currentHealth / maxHealth;
        }
    }
}
