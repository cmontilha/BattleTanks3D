using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bullet : MonoBehaviour
{
    public float moveSpeed = 7f; // Velocidade da Bullet
    private Vector3 destination;
    private Rigidbody rb;
    public int allowedCollisions = 3; // Colisões permitidas antes de destruir a Bullet
    private int currentCollisions = 0; // Contador de colisões

    // Start é chamado uma vez quando o script inicia
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Definir a velocidade inicial da Bullet em direção ao destino
        rb.linearVelocity = (destination - transform.position).normalized * moveSpeed;
        Invoke("disableTriggerCollider", 0.05f); // Desativa o Trigger logo após a inicialização
    }

    public void setDestination(Vector3 inputDestination)
    {
        destination = inputDestination;
    }

    public void disableTriggerCollider()
    {
        // Desativa o modo Trigger do Collider
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.isTrigger = false;
        }
    }

    // Lida com colisões
    void OnCollisionEnter(Collision collision)
    {
        // Verifica se a Bullet colidiu com uma parede
        if (collision.gameObject.tag == "Wall")
        {
            ContactPoint contact = collision.contacts[0];
            // Reflete a direção da Bullet com base no ponto de contato
            Vector3 reflectedDirection = Vector3.Reflect(rb.linearVelocity, contact.normal);
            rb.linearVelocity = reflectedDirection.normalized * moveSpeed; // Define a nova velocidade após a reflexão

            // Incrementa o contador de colisões
            currentCollisions++;

            // Verifica se o número máximo de colisões foi atingido
            if (currentCollisions >= allowedCollisions)
            {
                Destroy(gameObject); // Destroi a Bullet
            }
        }

        // Verifica se a Bullet colidiu com o tanque
        if (collision.gameObject.name == "TankTag" || collision.gameObject.name == "Tank")
        {
            Destroy(gameObject); // Destroi a Bullet
        }

        // Se colidiu com "PlayerTank", reinicia a cena
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