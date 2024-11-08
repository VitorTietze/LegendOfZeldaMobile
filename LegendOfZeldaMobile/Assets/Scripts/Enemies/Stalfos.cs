using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stalfos : Enemy
{
    protected override void Awake()
    {
        base.Awake();

        health = 2f;
        speed = 1.7f;
        damage = 1f;
    }

    private Coroutine movementPattern;
    private void OnBecameVisible()
    {
        movementPattern = StartCoroutine(MovementPattern());
    }

    private void OnBecameInvisible()
    {
        StopCoroutine(movementPattern);
    }

    private (float min, float max) movementChangeIntervalRange = (1.2f, 1.5f);
    private IEnumerator MovementPattern()
    {
        float interval;
        Vector2 randomDirection;
        while (true)
        {
            randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            rb.velocity = randomDirection * speed;

            interval = Random.Range(movementChangeIntervalRange.min, movementChangeIntervalRange.max);
            yield return new WaitForSeconds(interval);

            rb.velocity = (PlayerHealth.player.position - transform.position).normalized * speed;

            interval = Random.Range(movementChangeIntervalRange.min, movementChangeIntervalRange.max);
            yield return new WaitForSeconds(interval);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Walls")){
            rb.velocity *= -1;
        }
    }
}
