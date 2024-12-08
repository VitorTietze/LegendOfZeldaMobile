using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gel : Enemy
{
    protected override void SetStartingStats()
    {
        health = 1f;
        speed = 0.5f;
        damage = 1f;
    }

    private (float min, float max) movementChangeIntervalRange = (0.7f, 1.5f);
    protected override IEnumerator MovementPattern()
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

            Vector2 directionToPlayer = (PlayerHealth.player.position - transform.position).normalized;
            float jiggleFactor = Random.Range(-0.2f, 0.2f);
            Vector2 jigglyDirection = directionToPlayer + new Vector2(jiggleFactor, jiggleFactor);
            jigglyDirection = jigglyDirection.normalized;

            rb.velocity = jigglyDirection * speed;

            interval = Random.Range(movementChangeIntervalRange.min, movementChangeIntervalRange.max);
            yield return new WaitForSeconds(interval);
        }
    }
}
