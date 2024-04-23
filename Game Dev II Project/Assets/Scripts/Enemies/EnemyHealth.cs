using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private EnemyController enemyController;
    private BatBehaviour batBehaviour;
    private Rigidbody2D rb; // Corrected typo: Rigidbody2D instead of RigidBody2D

    public int maxHealth = 50;
    public int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
        enemyController = GetComponent<EnemyController>();
        batBehaviour = GetComponent<BatBehaviour>();
        rb = GetComponent<Rigidbody2D>(); // Corrected typo: Rigidbody2D instead of RigidBody2D
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Enemy Died");

        gameObject.layer = LayerMask.NameToLayer("DeadEnemy");
        gameObject.tag = "DeadEnemy";

        enemyController.StopMovement();
        batBehaviour.enabled = false;
        
        rb.velocity = Vector2.zero; // Stop the Rigidbody's velocity
    }
}
