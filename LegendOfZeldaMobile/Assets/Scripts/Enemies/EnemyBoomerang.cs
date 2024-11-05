using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoomerang : MonoBehaviour
{
    [SerializeField] private new Rigidbody2D rigidbody;
    private float speed = 2.5f;
    private float damage;
    private float flyingTime = 7f;
    private bool returning = false;

    private Vector2 spawnPosition;
    private Transform thrower;

    public void Initialize(Vector2 direction, Transform _thrower)
    {
        spawnPosition = transform.position;
        rigidbody.velocity = direction * speed;
        thrower = _thrower;
        StartCoroutine(CheckTrajectory());
    }

    private void Return()
    {
        rigidbody.velocity = -rigidbody.velocity;
        StartCoroutine(CheckIfReturned());
        returning = true;
    }

    private IEnumerator CheckTrajectory()
    {
        yield return new WaitForSeconds(flyingTime / 2);
        if (!returning) Return();
        yield return new WaitForSeconds(flyingTime / 2);
        Destroy(gameObject);
    }

    private IEnumerator CheckIfReturned()
    {
        while (true)
        {
            if (Vector2.Distance(transform.position, spawnPosition) < speed * 0.2f){
                // update thrower state
                Destroy(gameObject);
            }
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Walls"))
        {
            if (!returning) Return();
        }
    }
}
