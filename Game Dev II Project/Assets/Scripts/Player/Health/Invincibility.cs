using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invincibility : MonoBehaviour
{
    private PlayerHealth playerHealth;

    //=Link this to the player attack scsript.
    Renderer rend;
    Color c;
    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        rend = GetComponent<Renderer> ();
        c = rend.material.color;
    }


    //Change player health to whatever you named it.
    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("Collision");
        if(collider.gameObject.CompareTag("Enemy") && playerHealth.currentHealth > 0)
        {
            Debug.Log("Start Courintine");
            StartCoroutine("GetInvulnerable");
            Debug.Log("End Courintine");
        }
    }
    IEnumerator GetInvulnerable()
    {
        Physics2D.IgnoreLayerCollision (7, 8, true);
        c.a = 0.5f;
        rend.material.color = c;
        yield return new WaitForSeconds(2f);
        Physics2D.IgnoreLayerCollision (7, 8, false);
        c.a = 1f;
        rend.material.color = c;
    }
}