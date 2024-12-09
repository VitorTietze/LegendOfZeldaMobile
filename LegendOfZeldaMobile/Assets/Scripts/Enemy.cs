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

    private float knockbackDistance = 1.5f;
    private float knockbackDuration = 0.3f;
    public IEnumerator GetKnockedBack(Vector2 direction)
    {
        StopCoroutine(movementPattern);
        rb.velocity = direction * knockbackDistance / knockbackDuration;
        yield return new WaitForSeconds(knockbackDuration);
        rb.velocity = Vector2.zero;
        movementPattern = StartCoroutine(MovementPattern());
    }

    protected abstract IEnumerator MovementPattern();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Walls")){
            rb.velocity *= -1;
        }
    }
}
