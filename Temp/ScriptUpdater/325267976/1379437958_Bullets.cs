using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bullets : MonoBehaviour
{
    public float moveSpeed = 7f; // Velocidade de movimento da bullet
    private Vector3 destination;
    private Rigidbody rb;
    public int allowedCollisions = 2; // Número de colisões permitidas antes de destruir a bullet
    private int currentCollisions = 0; // Contador de colisões

    // Start é chamado antes da primeira atualização
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        if (rb == null)
        {
            Debug.LogError("Rigidbody não foi encontrado na Bullet!");
            return;
        }

        // Define a velocidade inicial da Bullet
        rb.linearVelocity = (destination - transform.position).normalized * moveSpeed;
        Invoke("disableTriggerCollider", 0.05f); // Desativa o Trigger logo após a inicialização
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
        // Desativa o modo Trigger do Collider
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.isTrigger = false;
        }
    }

    // Lida com as colisões
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Bullet colidiu com: " + collision.gameObject.name); // Log para monitorar com o que a Bullet colidiu

        if (collision.gameObject.tag == "Wall")
        {
            // Reflete a direção da bullet após o impacto com a parede
            ContactPoint contact = collision.contacts[0];
            Vector3 reflectedDirection = Vector3.Reflect(rb.linearVelocity, contact.normal);
            rb.linearVelocity = reflectedDirection.normalized * moveSpeed;

            currentCollisions++; // Incrementa o número de colisões

            Debug.Log("Colisão com parede. Contagem de colisões: " + currentCollisions); // Log para depuração

            // Verifica se atingiu o número máximo de colisões permitidas
            if (currentCollisions >= allowedCollisions)
            {
                Debug.Log("Número máximo de colisões atingido. Destruindo Bullet."); // Log de destruição
                Destroy(gameObject); // Destroi a Bullet
            }
        }

        // Reinicia a cena se a bullet colidir com o inimigo ou o tanque do jogador
        if (collision.gameObject.name == "Enemy" || collision.gameObject.name == "Tank")
        {
            Debug.Log("Colidiu com " + collision.gameObject.name + ". Reiniciando cena.");
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex);
        }
    }
}