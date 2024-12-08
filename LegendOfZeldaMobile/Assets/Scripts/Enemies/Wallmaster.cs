using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallmaster : Enemy
{
    [SerializeField] private Vector2 startingDirection;

    protected override void SetStartingStats()
    {
        health = 2f;
        speed = 1.5f;
        damage = 1f;
    }

    protected override void OnBecameVisible()
    {
        StartCoroutine(CheckIfLinkClose());
        //movementPattern = StartCoroutine(MovementPattern());
    }

    private float distanceThreshold = 3.5f; // arbitrary
    private float distanceToExitWall = 1.5f; // arbitrary
    private float timeToExitWall => distanceToExitWall / speed;
    private Transform link => GameManager.instance.link;
    private IEnumerator CheckIfLinkClose()
    {
        while (Vector2.Distance(link.position, transform.position) > distanceThreshold)
        {
            yield return new WaitForSeconds(0.2f);
        }

        Vector3 startingDirection2D = new Vector3(startingDirection.x, startingDirection.y, 0f);
        float angle = Mathf.Atan2(startingDirection2D.y, startingDirection2D.x) * Mathf.Rad2Deg;
        transform.Find("Sprite").rotation = Quaternion.Euler(0f, 0f, angle-90);

        rb.velocity = startingDirection * speed;
        yield return new WaitForSeconds(timeToExitWall);
        movementPattern = StartCoroutine(MovementPattern());
    }

    private (float min, float max) movementChangeIntervalRange = (1.5f, 1.9f);
    protected override IEnumerator MovementPattern()
    {
        StartCoroutine(EnableCollider());

        yield return new WaitForSeconds(Random.Range(0f, 0.35f));

        float interval;
        Vector2 randomDirection;
        while (true)
        {
            randomDirection = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1)).normalized;
            if (Mathf.Abs(randomDirection.x) > Mathf.Abs(randomDirection.y)){
                if (randomDirection.x > 0){
                    randomDirection = new Vector2(1f, 0f);
                } else {
                    randomDirection = new Vector2(-1f, 0f);
                }
            } else {
                if (randomDirection.y > 0){
                    randomDirection = new Vector2(0f, 1f);
                } else {
                    randomDirection = new Vector2(0f, -1f);
                }
            }
            rb.velocity = randomDirection * speed;

            interval = Random.Range(movementChangeIntervalRange.min, movementChangeIntervalRange.max);
            yield return new WaitForSeconds(interval);

            randomDirection = (PlayerHealth.player.position - transform.position).normalized;
            if (Mathf.Abs(randomDirection.x) > Mathf.Abs(randomDirection.y)){
                if (randomDirection.x > 0){
                    randomDirection = new Vector2(1f, 0f);
                } else {
                    randomDirection = new Vector2(-1f, 0f);
                }
            } else {
                if (randomDirection.y > 0){
                    randomDirection = new Vector2(0f, 1f);
                } else {
                    randomDirection = new Vector2(0f, -1f);
                }
            }
            rb.velocity = randomDirection * speed;

            interval = Random.Range(movementChangeIntervalRange.min, movementChangeIntervalRange.max);
            yield return new WaitForSeconds(interval);
        }
    }

    private float timeUntilEnablesCollider = 3f;
    private IEnumerator EnableCollider()
    {
        yield return new WaitForSeconds(timeUntilEnablesCollider);

        GetComponent<CircleCollider2D>().enabled = true;
    }
}
