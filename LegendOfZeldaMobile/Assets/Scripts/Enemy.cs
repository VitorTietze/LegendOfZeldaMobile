using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected float health;

    public void TakeDamage(float damage)
    {
        //Debug.Log($"{gameObject} took {damage} damage");

        health -= damage;
        if (health <= 0){
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
