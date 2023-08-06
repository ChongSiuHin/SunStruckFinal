using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

public class HealthBar : MonoBehaviour
{
    public GameObject player;
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

    public float minHealthPercentageForFlashing = 0.2f;
    public Color normalLightColor = Color.blue;
    public Color dangerLightColor = Color.red;
    public float flashDuration = 0.5f;

    private Coroutine flashCoroutine;

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

    private void UpdateLightColor()
    {
        if (currentHealth / maxHealth <= minHealthPercentageForFlashing)
        {
            if (flashCoroutine == null)
            {
                flashCoroutine = StartCoroutine(FlashLight());
            }
        }
        else
        {
            StopFlashing();
            playerLight.color = normalLightColor;
        }
    }

    private IEnumerator FlashLight()
    {
        while (true)
        {
            playerLight.color = dangerLightColor;
            yield return new WaitForSeconds(flashDuration);
            playerLight.color = normalLightColor;
            yield return new WaitForSeconds(flashDuration);
        }
    }

    private void StopFlashing()
    {
        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
            flashCoroutine = null;
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
        else if (currentHealth == 0)
        {
            player.transform.position = player.GetComponent<CheckpointRespawn>().respawnPoint;
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
            //playerLight.intensity = currentHealth / maxHealth;
            UpdateLightColor();
        }
    }
}
