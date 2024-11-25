using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public float maxHealth = 100f;  // Maximum health
    private float currentHealth;   // Current health
    public RectTransform healthBar; // Reference to the health bar UI

    void Start()
    {
        currentHealth = maxHealth; // Initialize health
        UpdateHealthBar();         // Sync UI
    }

    // Method to reduce health
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
        UpdateHealthBar();
    }

    // Update the health bar UI
    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.localScale = new Vector3(currentHealth / maxHealth, 1, 1);
        }
    }

    // Character dies
    void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        // Add death behavior, such as disabling the character
        Destroy(gameObject); // Example: Destroy the character
    }
}
