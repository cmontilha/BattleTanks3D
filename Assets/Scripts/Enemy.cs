//Caio Oct 20, 2024
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : BaseTank
{
    public float shootInterval = 3.0f; // Shooting interval
    private GameObject playerTank;      // Reference to the player's tank
    private NavMeshAgent agent;         // Navigation agent for AI movement

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // Get the NavMeshAgent component to control the enemy's movement
        playerTank = GameObject.Find("Tank");  // Associate the destination with the player's tank object

        // Check if essential components have been assigned
        if (bulletPrefab == null || bulletSpawnPoint == null)
        {
            Debug.LogError("bulletPrefab or bulletSpawnPoint is not assigned in Enemy!");
            return;
        }

        StartCoroutine(ShootRoutine(shootInterval)); // Start the shooting routine
    }

    // Update is called once per frame
    void Update()
    {
        // The enemy moves towards the player's tank
        if (playerTank != null)
        {
            agent.destination = playerTank.transform.position;

            // Ensures the enemy looks towards the player while moving
            Vector3 direction = playerTank.transform.position - transform.position;
            direction.y = 0; // Keep the rotation on the XZ plane
            if (direction != Vector3.zero)
            {
                Quaternion rotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10f);
            }
        }
    }

    // Coroutine to shoot bullets towards the player's tank at regular intervals
    IEnumerator ShootRoutine(float duration)
    {
        while (true) // Infinite loop for the shooting routine
        {
            yield return new WaitForSeconds(duration); // Wait for the defined time before shooting

            if (playerTank != null) // Check if the player's tank is still active
            {
                Debug.Log("Enemy fired a bullet towards the Tank.");

                // Use the method from the BaseTank class to shoot a bullet towards the player's tank
                base.shootBullet(playerTank.transform.position);
            }
        }
    }
}