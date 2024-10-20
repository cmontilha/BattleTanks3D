//Caio Oct 20, 2024
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
    public float moveSpeed = 5f; // Movement speed
    public float rotationSpeed = 10f; // Rotation speed
    
    // Reference to the bullet prefab
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    
    // Start is called before the first frame update
    void Start()
    {
        // Initializations, if necessary
    }

    // Update is called once per frame
    void Update()
    {
        // Get input from arrow keys or WASD
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Adjust the direction so that the tank moves forward correctly
        Vector3 movementDirection = transform.forward * (-verticalInput) + transform.right * horizontalInput;

        // Move the tank based on the calculated direction
        transform.Translate(movementDirection * moveSpeed * Time.deltaTime, Space.World);

        // Get the current mouse position in screen coordinates
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.transform.position.y; // Set the distance of the camera from the object

        // Convert the mouse position from screen space to world space
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);

        // Calculate the direction for the tank to face the mouse
        Vector3 direction = transform.position - worldMousePos; // Here we invert the direction for the tank to look at the mouse
        direction.y = 0; // Keep the tank leveled on the Y-axis

        // Calculate the rotation to look at the mouse
        Quaternion rotation = Quaternion.LookRotation(direction);

        // Rotate the tank smoothly towards the mouse direction
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

        // Check if the left mouse button was clicked
        if (Input.GetMouseButtonDown(0))
        {
            ShootAtMousePosition();
        }
    }

    void ShootAtMousePosition()
    {
        // Instantiate the bullet at the spawn point
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

        // Get the mouse position where the shot will be directed
        Vector3 bulletTargetPos = Input.mousePosition;
        bulletTargetPos.z = Camera.main.transform.position.y;

        Vector3 finalDestination = Camera.main.ScreenToWorldPoint(bulletTargetPos);

        // Calculate the direction of the shot
        Vector3 shootDirection = (finalDestination - bullet.transform.position).normalized;

        // Apply force to the bullet (assuming the bullet has a Rigidbody)
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        bulletRb.AddForce(shootDirection * 1000f); // Adjust the force as necessary
    }
}
