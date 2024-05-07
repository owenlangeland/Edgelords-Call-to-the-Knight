using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour, IDataPersistence
{
    public int maxHealth = 100;
    public int currentHealth;

    public HealthBar healthBar;

    public LayerMask enemyLayers;

    // Start is called before the first frame update
    void Start()
    {
        healthBar.SetMaxHealth(maxHealth);
    }

    public void loadData(GameData data)
    {
        this.currentHealth = data.currentHealth;
    }
    public void SaveData(ref GameData data)
    {
        data.currentHealth = this.currentHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            SceneManager.LoadScene("DeathScreen");
        }

        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(20);
        }
        */
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Player Taken Damage");
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        // Check if the collider's layer is included in the enemyLayers mask
        if (((1 << collider.gameObject.layer) & enemyLayers) != 0)
        {
            // If collided with an enemy layer, take damage
            TakeDamage(20);
        }
    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        // Check if the collider's layer is included in the enemyLayers mask
        if (((1 << collider.gameObject.layer) & enemyLayers) != 0)
        {
            // If collided with an enemy layer, take damage
            TakeDamage(20);
        }
    }
}
