using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{

    //Animations
    public Animator animator;

    //Attack hitbox and detecting enemies 
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

    // player damage ammount can be change if having code added with weapon later on 
    public int attackDamage = 20;

    //Attack Rate
    public float attackRate = 0.5f;
    float nextAttackTime = 0f;

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetButtonDown("Attack"))// comand key for attacking 
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }


    void Attack()
    {
        // play attack aninamation
        animator.SetTrigger("Attack");

        // detect enemies in attack range
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // damage enemies 
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyDamage>().TakeDamage(attackDamage);//taking the components of the enemies hp and making them take damage code needs to be changed later when more enemies are added along with code names EnemyDamage will have to change
            Debug.Log("Enemies is hurt");
        }


    }
    // to see hit box
    void OnDrawGizmosSelected()
    {

        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}