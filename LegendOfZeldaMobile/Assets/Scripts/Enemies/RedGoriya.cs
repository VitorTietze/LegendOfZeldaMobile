using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedGoriya : Enemy
{
    private AnimatedSpritePlus animatedPlus;
    private GameObject boomerang;

    protected override void Awake()
    {
        base.Awake();

        animatedPlus = transform.Find("Sprite").GetComponent<AnimatedSpritePlus>();
        boomerang = Resources.Load<GameObject>("Prefabs/EnemyBoomerang");
    }

    protected override void SetStartingStats()
    {
        health = 3f;
        speed = 1.8f;
        damage = 1f;
    }

    private bool canMove = true;
    private (float min, float max) movementChangeIntervalRange = (1.55f, 2.15f);
    protected override IEnumerator MovementPattern()
    {
        StartCoroutine(AttackCheck());

        yield return new WaitForSeconds(Random.Range(0f, 0.35f));

        float interval;
        Vector2 randomDirection;
        while (canMove)
        {
            randomDirection = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1)).normalized;
            if (Mathf.Abs(randomDirection.x) > Mathf.Abs(randomDirection.y)){
                randomDirection = new Vector2(1f, 0f);
            } else {
                randomDirection = new Vector2(0f, 1f);
            }
            animatedPlus.horizontal = (int)randomDirection.x;
            animatedPlus.vertical = (int)randomDirection.y;
            rb.velocity = randomDirection * speed;

            interval = Random.Range(movementChangeIntervalRange.min, movementChangeIntervalRange.max);
            yield return new WaitForSeconds(interval);

            /* interval = Random.Range(movementChangeIntervalRange.min, movementChangeIntervalRange.max);
            rb.velocity = Vector2.zero;
            animatedPlus.running = canMove ? false : animatedPlus.running;
            yield return new WaitForSeconds(interval); */

            randomDirection = (PlayerHealth.player.position - transform.position).normalized;
            if (Mathf.Abs(randomDirection.x) > Mathf.Abs(randomDirection.y)){
                randomDirection = new Vector2(1f, 0f);
            } else {
                randomDirection = new Vector2(0f, 1f);
            }
            animatedPlus.horizontal = (int)randomDirection.x;
            animatedPlus.vertical = (int)randomDirection.y;
            rb.velocity = randomDirection * speed;

            interval = Random.Range(movementChangeIntervalRange.min, movementChangeIntervalRange.max);
            yield return new WaitForSeconds(interval);
        }
    }

    private Transform link => GameManager.instance.link;
    private IEnumerator AttackCheck()
    {
        float width = 1.0f;
        Vector2 throwDirection = Vector2.zero;

        while (canMove)
        {
            Vector2 directionToLink = link.position - transform.position;

            if (Mathf.Abs(directionToLink.x) <= width &&
                Mathf.Abs(directionToLink.y) > Mathf.Abs(directionToLink.x)){
                throwDirection = directionToLink.y > 0 ? Vector2.up : Vector2.down;
            }
            else if (Mathf.Abs(directionToLink.y) <= width &&
                Mathf.Abs(directionToLink.x) > Mathf.Abs(directionToLink.y)){
                throwDirection = directionToLink.x > 0 ? Vector2.right : Vector2.left;
            }

            if (throwDirection != Vector2.zero){
                canMove = false;
                rb.velocity = Vector2.zero;
                animatedPlus.running = false;
                animatedPlus.attacking = true;
                //Debug.Log($"Attacking in direction: {throwDirection}");
                animatedPlus.horizontal = (int)throwDirection.x;
                animatedPlus.vertical = (int)throwDirection.y;
                ThrowBoomerang(throwDirection);
            }

            throwDirection = Vector2.zero;
            yield return new WaitForSeconds(0.25f);
        }
    }

    public void ThrowBoomerang(Vector2 direction)
    {
        Vector2 adjustedPosition = (Vector2)transform.position + direction * 0.2f;
        Transform rangs = transform.Find("Boomerangs");
        GameObject boomerangObj = Instantiate(boomerang, adjustedPosition, Quaternion.identity, rangs);
        boomerangObj.GetComponent<EnemyBoomerang>().Initialize(direction, transform);
    }

    public void BoomerangReturned()
    {
        StartCoroutine(ThrowDelayAfterReturning());
    }

    private IEnumerator ThrowDelayAfterReturning()
    {
        yield return new WaitForSeconds(2.5f);
        canMove = true;
        animatedPlus.running = true;
        animatedPlus.attacking = false;
        StartCoroutine(MovementPattern());
    }
}
