using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] private new Rigidbody2D rigidbody;
    private float speed = 2.65f;
    private float damage;

    public void Initialize(Vector2 direction, float originalDamage, float angleChange)
    {
        float radians = angleChange * Mathf.Deg2Rad;
        float cos = Mathf.Cos(radians);
        float sin = Mathf.Sin(radians);
        direction = new Vector2(direction.x * cos - direction.y * sin, direction.x * sin + direction.y * cos);

        damage = originalDamage;
        rigidbody.velocity = direction * speed;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Walls")){
            Destroy(gameObject);
        }

        Transform parentOther = other.transform.parent;
        if (parentOther != null){
            if (parentOther.gameObject.TryGetComponent<PlayerHealth>(out PlayerHealth playerHealth)){
                playerHealth.TakeDamage(damage);
                playerHealth.touchingEnemy = false;
                Destroy(gameObject);
            }
        }
    }
}
