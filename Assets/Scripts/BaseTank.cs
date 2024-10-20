//Caio Oct 20, 2024
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTank : MonoBehaviour
{
    public GameObject bulletPrefab;     // Bullet prefab
    public Transform bulletSpawnPoint;  // Bullet spawn point
    public float bulletSpeed = 1000f;   // Bullet speed

    // Method to shoot a bullet in a specific direction
    public void shootBullet(Vector3 targetPosition)
    {
        // Instantiate the bullet at the spawn point
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

        // Calculate the direction of the shot
        Vector3 shootDirection = (targetPosition - bulletSpawnPoint.position).normalized;

        // Apply force to the bullet (assuming the bullet has a Rigidbody)
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        bulletRb.AddForce(shootDirection * bulletSpeed); // Adjust the force as needed
    }
}