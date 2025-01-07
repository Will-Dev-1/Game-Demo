using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class idleEnemy : MonoBehaviour
{
    public float health = 100f; 
    public float chaseDistance = 10f;
    public float attackDistance = 5f;
    public float projectileSpeed = 10f;
    public float fireRate = 1f;
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public UnityEngine.AI.NavMeshAgent agent;
    public Transform player;
    public Transform helper;
    private float nextFireTime = 0f;
    private int hitCount = 0; 

    public GameObject healthBarPrefab;
    private Slider healthBar; 

    private void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        helper = GameObject.FindGameObjectWithTag("Helper").transform; 

        // Instantiate the health bar
        if (healthBarPrefab != null)
        {
            GameObject healthBarInstance = Instantiate(healthBarPrefab, transform.position + Vector3.up * 2, Quaternion.identity, transform);
            healthBar = healthBarInstance.GetComponentInChildren<Slider>();
            healthBar.maxValue = 3; // Set the max value to match the number of hits needed
            healthBar.value = 3; // Start at full health (3 hits remaining)
        }
    }

    private void Update()
    {
        float distanceToHelper = helper != null ? Vector3.Distance(transform.position, helper.position) : Mathf.Infinity;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Prioritize attacking helper if it's within chase and attack range
        if (helper != null && distanceToHelper <= chaseDistance)
        {
            agent.SetDestination(helper.position);

            if (distanceToHelper <= attackDistance && Time.time >= nextFireTime)
            {
                nextFireTime = Time.time + 1f / fireRate;

                Vector3 projectileDirection = (helper.position - projectileSpawnPoint.position).normalized;
                GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
                projectile.GetComponent<Rigidbody>().velocity = projectileDirection * projectileSpeed;
            }
        }
        // If the helper isn't present or isn't in range, attack the player
        else if (distanceToPlayer <= chaseDistance)
        {
            agent.SetDestination(player.position);

            if (distanceToPlayer <= attackDistance && Time.time >= nextFireTime)
            {
                nextFireTime = Time.time + 1f / fireRate;

                Vector3 projectileDirection = (player.position - projectileSpawnPoint.position).normalized;
                GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
                projectile.GetComponent<Rigidbody>().velocity = projectileDirection * projectileSpeed;
            }
        }
        else
        {
            agent.SetDestination(transform.position); // Idle if no target is within range
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        hitCount++; // Increment the hit counter

        // Update the health bar
        if (healthBar != null)
        {
            healthBar.value = 3 - hitCount; 
        }

        if (hitCount >= 3)
        {
            Destroy(gameObject); // Destroy the enemy after 3 hits
        }

        if (health <= 0)
        {
            Destroy(gameObject); 
        }
    }
}
