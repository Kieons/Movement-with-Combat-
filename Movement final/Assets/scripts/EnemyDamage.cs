using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    // HP
    public int maxHealth = 100;
    int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth; // what your current health is equal to
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage; // damage calculation, current hp minus damage taken 

        //play hurt animation

        if (currentHealth <= 0)
        {
            Die();
        }
    }


    void Die()
    {
        Debug.Log("Enemy Died!");
        // Death animation
        // Disable enemy or make is disapear
    }
}