using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bullet : MonoBehaviour
{
    public float moveSpeed = 7f; // Speed of the movement
    private Vector3 destination;
    private Rigidbody rb;
    public int allowedCollisions = 3;
    private int currentCollisions = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = (destination - transform.position).normalized * moveSpeed;
        Invoke("disableTriggerCollider", 0.05f);
    }

    public void setActiveBullet(bool active){
        gameObject.SetActive(active);
    }

    public void setDestination(Vector3 inputDestination){
        destination = inputDestination;
    }

    public Vector3 getDestination(){
        return destination;
    }
    public void disableTriggerCollider(){
        transform.GetComponent<Collider>().isTrigger = false;
    }
    


    void OnCollisionEnter(Collision collision)
    {   
        if (collision.gameObject.tag == "Wall")
        {
           ContactPoint contact = collision.contacts[0];
           Vector3 direction = Vector3.Project(rb.linearVelocity, contact.normal);
           GetComponent<Rigidbody>().AddForce(direction * moveSpeed);
           currentCollisions++;
           if(currentCollisions >= allowedCollisions){
                Destroy(gameObject);
           }
        }

        if (collision.gameObject.name == "TankTag")
        {
            Destroy(gameObject);
        }

        if(collision.gameObject.name == "PlayerTank"){
            // Get the current scene index
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

            // Reload the current scene
            SceneManager.LoadScene(currentSceneIndex);
        }
            
   
    }

    // Update is called once per frame
    void Update()
    {

    }
}