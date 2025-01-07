using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public Slider healthSlider;
    public AudioSource audioSource;
    public AudioClip pickupSound;

    void Start()
    {
        currentHealth = startingHealth;
        healthSlider.value = currentHealth;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            currentHealth -= 5;
            healthSlider.value = currentHealth;

            if (currentHealth <= 0)
            {
                // Handle player death
            }
        }
        else if (other.CompareTag("Health10"))
        {
            currentHealth += 10;
            healthSlider.value = currentHealth;
            PlayPickupSound();
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Health20"))
        {
            currentHealth += 20;
            healthSlider.value = currentHealth;
            PlayPickupSound();
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Health30"))
        {
            currentHealth += 30;
            healthSlider.value = currentHealth;
            PlayPickupSound();
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Projectile"))
        {
            currentHealth -= 10;
            healthSlider.value = currentHealth;
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Ammo"))
        {
            PlayPickupSound();
            Destroy(other.gameObject);
        }
    }

    void PlayPickupSound()
    {
        if (audioSource != null && pickupSound != null)
        {
            audioSource.PlayOneShot(pickupSound);
        }
    }
}
