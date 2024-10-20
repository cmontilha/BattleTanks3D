using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bullet : MonoBehaviour
{
    public float moveSpeed = 7f; // Velocidade de movimento
    private Vector3 destination;
    private Rigidbody rb;
    public int allowedCollisions = 2; // Número de colisões permitidas antes de destruir o projétil
    private int currentCollisions = 0; // Contador de colisões

    // Start é chamado antes da primeira atualização
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = (destination - transform.position).normalized * moveSpeed;
        Invoke("disableTriggerCollider", 0.05f);
    }

    public void setActiveBullet(bool active)
    {
        gameObject.SetActive(active);
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
        GetComponent<Collider>().isTrigger = false;
    }

    // Lida com colisões
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            // Calcula a nova direção da bala após o impacto com a parede
            ContactPoint contact = collision.contacts[0];
            Vector3 direction = Vector3.Reflect(rb.linearVelocity, contact.normal);
            rb.linearVelocity = direction.normalized * moveSpeed;

            // Incrementa o contador de colisões
            currentCollisions++;

            // Verifica se o número máximo de colisões foi atingido
            if (currentCollisions >= allowedCollisions)
            {
                Destroy(gameObject); // Destroi a bala
            }
        }

        // Destrói a bala ao colidir com o inimigo
        if (collision.gameObject.name == "Enemy")
        {
            Destroy(gameObject);
        }

        // Reinicia a cena se a bala colidir com o tanque
        if (collision.gameObject.name == "Tank")
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex);
        }
    }

    // Update é chamado uma vez por frame
    void Update()
    {
        // Deixe o Update vazio, a menos que haja algo específico a ser executado aqui
    }
}