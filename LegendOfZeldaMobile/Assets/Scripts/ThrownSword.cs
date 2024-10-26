using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownSword : MonoBehaviour
{
    [SerializeField] private new Rigidbody2D rigidbody;
    private float speed = 4f; // arbitrary
    public float damage;
    private float despawnTime = 8f;

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemies"))
        {
            other.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            ConsumeSword();
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Walls"))
        {
            ConsumeSword();
        }
    }
}
