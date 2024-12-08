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

        SetStartingStats();
    }

    protected abstract void SetStartingStats();

    public void TakeDamage(float damage)
    {
        Debug.Log($"{gameObject} took {damage} damage.");
        health -= damage;
        if (health <= 0){
            Die();
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    protected Coroutine movementPattern;
    protected virtual void OnBecameVisible()
    {
        movementPattern = StartCoroutine(MovementPattern());
    }

    private void OnBecameInvisible()
    {
        StopCoroutine(movementPattern);
    }

    protected abstract IEnumerator MovementPattern();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Walls")){
            rb.velocity *= -1;
        }
    }
}
