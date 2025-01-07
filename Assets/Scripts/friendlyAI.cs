using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class friendlyAI : MonoBehaviour
{
    public GameObject player;
    public float followDistance = 5f;
    public float attackRange = 10f;
    public float projectileSpeed = 15f;
    public GameObject projectilePrefab;
    public Transform firePoint; 
    public float fireRate = 1f;
    private float nextFireTime = 0f;

    //health of the AI
    public float maxHealth = 100f; 
    private float currentHealth;
    public Slider healthBar; 

    private UnityEngine.AI.NavMeshAgent navAgent;

    void Start()
    {
        navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        navAgent.stoppingDistance = followDistance;
        currentHealth = maxHealth; 
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }

    void Update()
    {
        // Follow player
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer > followDistance)
        {
            navAgent.SetDestination(player.transform.position);
        }

        // Find nearest enemy within range
        GameObject nearestEnemy = FindNearestEnemy();
        if (nearestEnemy != null)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, nearestEnemy.transform.position);

            // Face and attack enemy if within range
            if (distanceToEnemy <= attackRange)
            {
                FaceTarget(nearestEnemy.transform);

                if (Time.time >= nextFireTime)
                {
                    nextFireTime = Time.time + 1f / fireRate;
                    FireProjectile(nearestEnemy.transform);
                }
            }
        }

        // Update the health bar
        healthBar.value = currentHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthBar.value = currentHealth; 

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Handle AI death 
        Destroy(gameObject);
        Debug.Log("Friendly AI has died.");
    }

    private GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearestEnemy = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < shortestDistance && distance <= attackRange)
            {
                shortestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }

    private void FaceTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void FireProjectile(Transform target)
    {
        Vector3 projectileDirection = (target.position - firePoint.position).normalized;
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        projectile.GetComponent<Rigidbody>().velocity = projectileDirection * projectileSpeed;

        
    }
}
