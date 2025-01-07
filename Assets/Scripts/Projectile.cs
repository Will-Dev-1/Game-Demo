using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 5f;
    public float damage = 10f; 

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if projectile collides with enemy
        if (other.CompareTag("Enemy"))
        {
            idleEnemy enemyScript = other.GetComponent<idleEnemy>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(damage); // Apply damage to the enemy
                Destroy(gameObject); // Destroy the projectile after hitting the enemy
            }
        }
        // Check if the projectile collides with the friendly AI
        else if (other.CompareTag("Helper"))
        {
            friendlyAI aiScript = other.GetComponent<friendlyAI>();
            if (aiScript != null)
            {
                aiScript.TakeDamage(damage); // Apply damage to the friendly AI
                Destroy(gameObject); // Destroy the projectile after hitting the friendly AI
            }
        }
    }
}
