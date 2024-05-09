using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyHealth : MonoBehaviour
{
    private Animator animator;
    private EnemyController enemyController;
    private BatBehaviour batBehaviour;
    private BossScript bossScript;
    private Rigidbody2D rb; // Corrected typo: Rigidbody2D instead of RigidBody2D

    public int maxHealth = 50;
    public int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        enemyController = GetComponent<EnemyController>();
        batBehaviour = GetComponent<BatBehaviour>();
        bossScript = GetComponent<BossScript>();
        rb = GetComponent<Rigidbody2D>(); // Corrected typo: Rigidbody2D instead of RigidBody2D
    }

    public void TakeDamage(int damage)
    {
        GetComponent<DamageFlash>().Flash();
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Enemy Died");

        animator.SetBool("Exit", true);

        gameObject.layer = LayerMask.NameToLayer("DeadEnemy");
        gameObject.tag = "DeadEnemy";
        

        if (enemyController != null) {
            enemyController.StopMovement();
            rb.gravityScale = 1f;
        }

        if (batBehaviour != null)
            batBehaviour.enabled = false;

        if (bossScript != null)
            bossScript.enabled = false;

        if (bossScript != null && currentHealth <= 0)
        {
            Debug.Log("Win");
            StartCoroutine("WinScreen");
        }

        if (bossScript == null)
            StartCoroutine(DeleteEnemy()); 
        
        rb.velocity = Vector2.zero; 
    }

    private IEnumerator DeleteEnemy()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject); // Changed 'Delete' to 'Destroy' to remove the GameObject from the scene
    }

    private IEnumerator WinScreen()
    {
        Debug.Log("Win Screen Activated");
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("WinScreen");
    }
}
