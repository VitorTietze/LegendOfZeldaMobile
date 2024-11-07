using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected Rigidbody2D rb;

    protected float health;
    protected float speed;
    public float damage;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void TakeDamage(float damage)
    {
        //Debug.Log($"{gameObject} took {damage} damage");
        health -= damage;
        if (health <= 0){
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
