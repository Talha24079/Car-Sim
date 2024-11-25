using UnityEngine;

public class CarDamage : MonoBehaviour
{
    public float damage = 20f; // Damage inflicted by the car

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object has a HealthManager
        HealthManager healthManager = collision.gameObject.GetComponent<HealthManager>();
        if (healthManager != null)
        {
            healthManager.TakeDamage(damage); // Apply damage
        }
    }
}
