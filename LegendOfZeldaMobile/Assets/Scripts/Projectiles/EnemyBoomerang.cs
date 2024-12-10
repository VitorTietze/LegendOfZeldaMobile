using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoomerang : MonoBehaviour
{
    [SerializeField] private new Rigidbody2D rigidbody;
    private float speed = 2.6f;
    private float damage = 1f;
    private float flyingTime = 6.3f;
    private bool returning = false;

    private Vector2 spawnPosition;
    private Transform thrower;

    public void Initialize(Vector2 direction, Transform _thrower)
    {
        spawnPosition = transform.position;
        rigidbody.velocity = direction * speed;
        thrower = _thrower;
        StartCoroutine(CheckTrajectory());
        StartCoroutine(SpinSprite());
    }

    private float framerate = 6f;
    private float frametime => 1f / framerate;
    private IEnumerator SpinSprite()
    {
        Transform spriteTransform = transform.Find("Sprite");

        while (true)
        {
            spriteTransform.rotation *= Quaternion.Euler(0f, 0f, 90f);

            yield return new WaitForSeconds(frametime);
        }
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
                thrower.GetComponent<RedGoriya>().BoomerangReturned();
                Destroy(gameObject);
            }
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Walls")){
            if (!returning) Return();
        }

        Transform parentOther = other.transform.parent;
        if (parentOther != null){
            if (parentOther.gameObject.TryGetComponent<PlayerHealth>(out PlayerHealth playerHealth)){
                playerHealth.TakeDamage(damage);
                if (!returning) Return();
            }
        }
    }
}
