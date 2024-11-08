using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownSword : MonoBehaviour
{
    [SerializeField] private new Rigidbody2D rigidbody;
    private float speed = 4f; // arbitrary
    private float damage;
    private float despawnTime = 4.5f;

    public void Initialize(Vector2 direction, float originalDamage)
    {
        damage = originalDamage;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3 (0f, 0f, angle + 270f);
        rigidbody.velocity = direction * speed;

        StartCoroutine(Despawn());
    }

    private void ConsumeSword()
    {
        Destroy(gameObject);
    }

    private IEnumerator Despawn()
    {
        yield return new WaitForSeconds(despawnTime);
        Destroy(gameObject);
    }

    /* private void DealDamage(GameObject hitObject)
    {
        hitObject.GetComponent<Enemy>()?.TakeDamage(damage);
    } */

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
        {
            //DealDamage(other.gameObject);
            enemy.TakeDamage(damage);
            ConsumeSword();
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Walls"))
        {
            ConsumeSword();
        }
    }
}
