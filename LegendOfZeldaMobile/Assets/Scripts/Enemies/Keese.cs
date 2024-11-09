using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keese : Enemy
{
    protected override void Awake()
    {
        base.Awake();

        health = 1f;
        speed = 1.5f;
        damage = 0.5f;
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

    private (float min, float max) movementChangeIntervalRange = (0.9f, 1.8f);
    private IEnumerator MovementPattern()
    {
        yield return new WaitForSeconds(Random.Range(0f, 0.35f));

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
