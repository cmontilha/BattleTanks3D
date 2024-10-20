//Caio Oct 20, 2024
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bullets : MonoBehaviour
{
    public float moveSpeed = 7f; // Bullet movement speed
    private Vector3 destination;
    private Rigidbody rb;
    public int allowedCollisions = 2; // Number of collisions allowed before the bullet is destroyed
    private int currentCollisions = 0; // Collision counter

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        if (rb == null)
        {
            Debug.LogError("Rigidbody not found on Bullet!");
            return;
        }

        // Sets the initial speed of the Bullet
        rb.linearVelocity = (destination - transform.position).normalized * moveSpeed;
        Invoke("disableTriggerCollider", 0.05f); // Disables the trigger shortly after initialization
    }

    public void setDestination(Vector3 inputDestination)
    {
        destination = inputDestination;
    }

    public Vector3 getDestination()
    {
        return destination;
    }

    public void disableTriggerCollider()
    {
        // Disables the trigger mode of the collider
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.isTrigger = false;
        }
    }

    // Handles collisions
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Bullet collided with: " + collision.gameObject.name); // Log to monitor what the bullet collided with

        if (collision.gameObject.tag == "Wall")
        {
            // Reflects the bullet's direction after impacting a wall
            ContactPoint contact = collision.contacts[0];
            Vector3 reflectedDirection = Vector3.Reflect(rb.linearVelocity, contact.normal);
            rb.linearVelocity = reflectedDirection.normalized * moveSpeed;

            currentCollisions++; // Increments the number of collisions

            Debug.Log("Collision with wall. Collision count: " + currentCollisions); // Debug log

            // Checks if the maximum number of allowed collisions has been reached
            if (currentCollisions >= allowedCollisions)
            {
                Debug.Log("Maximum number of collisions reached. Destroying Bullet."); // Destruction log
                Destroy(gameObject); // Destroys the Bullet
            }
        }

        // Reloads the scene if the bullet collides with the enemy or the player's tank
        if (collision.gameObject.name == "Enemy" || collision.gameObject.name == "Tank")
        {
            Debug.Log("Collided with " + collision.gameObject.name + ". Reloading scene.");
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex);
        }
    }
}