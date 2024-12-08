using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aquamentus : Enemy
{
    private GameObject fireball;

    protected override void Awake()
    {
        base.Awake();

        fireball = Resources.Load<GameObject>("Prefabs/Fireball");
    }

    protected override void SetStartingStats()
    {
        health = 8f;
        speed = 0.5f;
        damage = 1.5f;
    }

    protected override IEnumerator MovementPattern()
    {
        StartCoroutine(AttackPattern());

        while (true)
        {
            rb.velocity = Vector2.left * speed;
            yield return new WaitForSeconds(1f);
            rb.velocity = Vector2.right * speed;
            yield return new WaitForSeconds(1f);
        }
    }

    private float timeBeforeAttacking = 3f;
    private float throwInterval = 3.5f;
    private float maxAngDev = 15f;
    private float secondaryFireballsDev = 20f; // TEST THIS FIELD
    private Transform link => GameManager.instance.link;
    private IEnumerator AttackPattern()
    {
        yield return new WaitForSeconds(timeBeforeAttacking);

        while (true)
        {
            float randomAngDev = Random.Range(-maxAngDev, maxAngDev);
            Vector2 throwDirection = (link.position - transform.position).normalized;
            ThrowFireball(throwDirection, randomAngDev);
            ThrowFireball(throwDirection, +secondaryFireballsDev + randomAngDev);
            ThrowFireball(throwDirection, -secondaryFireballsDev + randomAngDev);

            yield return new WaitForSeconds(throwInterval);
        }
    }

    public void ThrowFireball(Vector2 direction, float angleChange = 0f)
    {
        Vector2 adjustedPosition = (Vector2)transform.position + direction * 0.2f;
        GameObject fireballObj = Instantiate(fireball, adjustedPosition, Quaternion.identity, transform);
        fireballObj.GetComponent<Fireball>().Initialize(direction, damage, angleChange);
    }
}
