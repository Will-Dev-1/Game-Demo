using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject[] projectilePrefabs; // Array to hold different projectile prefabs
    public Transform firePoint; 
    public float fireRate = 0.5f; 

    private float nextFireTime = 0f;
    private int currentProjectileIndex = 0; // Index to track the current projectile

    void Update()
    {
        // Check for shooting input and if fire cooldown has elapsed
        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Shoot(currentProjectileIndex);
        }

        // Cycle through projectiles with number keys (e.g., 1, 2, 3)
        if (Input.GetKeyDown(KeyCode.Alpha1)) currentProjectileIndex = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2)) currentProjectileIndex = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3)) currentProjectileIndex = 2;
    }

    void Shoot(int projectileIndex)
    {
        if (projectileIndex >= 0 && projectileIndex < projectilePrefabs.Length)
        {
            Instantiate(projectilePrefabs[projectileIndex], firePoint.position, firePoint.rotation);
        }
    }
}
