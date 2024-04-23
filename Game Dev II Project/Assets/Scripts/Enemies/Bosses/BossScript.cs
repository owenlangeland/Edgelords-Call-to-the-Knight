using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public Animator animator;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
   
    private int attacks;
    private int basicAttack = 0;
    private int randomint;

    private int Weight1;
    private int Weight2;
    private bool twice = false;
    private Rigidbody2D rb;
    private bool rush = false;
    public float forceAmount = 100f;

    public float attackCooldown = 2f; // Cooldown time between attacks
    private float currentCooldown = 0f; // Current cooldown progress
    private float initialDelayTimer = 5f;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
         if (initialDelayTimer > 0)
        {
            initialDelayTimer -= Time.deltaTime;
        }
        else
        {
             if (currentCooldown > 0)
            {
                currentCooldown -= Time.deltaTime;
            }
            else
            {
                // If the cooldown is finished, perform the attack and reset the cooldown
                Boss();
                currentCooldown = attackCooldown;
            }
        }   
        
    }

    private void Boss()
    {
        randomAttack();
        if(attacks == 1)
        {
             Debug.Log("1");
            animator.SetTrigger("isAttacking");
            Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
            foreach(Collider2D player in hitPlayer)
            {
                
                playerHealth.TakeDamage(20);
               
            }
            
        }
        else if(attacks == 2)
        {
            Debug.Log("2");
            if(rush == false)
            {
                rb.AddForce(Vector2.left * forceAmount, ForceMode2D.Impulse);
                flip();
                rush = true;
            }
            else if(rush)
            {
                rb.AddForce(Vector2.right * forceAmount, ForceMode2D.Impulse);
                flip();
                rush = false;
            }
            
        }
    }

    private void randomAttack()
    {
        Weight1 = basicAttack + 40;
        Weight2 = 80;

        if (twice == true)
        {
            Debug.Log("Twice");
            attacks = 2;
            twice = false;
        }
        else if (attacks == 1)
        {
            if (basicAttack == 30)
            {
                basicAttack = -10;
            }
            else
            {
                basicAttack = basicAttack * 10;
            }
        }
        else if (attacks == 2)
        {
            randomint = Random.Range(1, 4);
            if (randomint == 4)
            {
                twice = true;
            }
        }
        else
        {
            randomint = Random.Range(1, 3);
        }

        attackPattern();
    }

    private void attackPattern()
    {
        randomint = Random.Range(0, Weight2);
        if (randomint <= Weight1)
        {
            attacks = 1;
        }
        else
        {
            attacks = 2;
        }
    }
    private void flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}