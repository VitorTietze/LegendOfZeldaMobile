using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerHealth playerHealth;
    private float damage = 3f; // arbitrary
    private float width = 1f;
    private float depth = 1f;
    private Vector2 direction;
    private LayerMask layerMask;

    private void Awake()
    {
        layerMask = LayerMask.NameToLayer("Enemies");
    }

    public void SwordAttack()
    {
        direction = playerMovement.movementDirection;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Vector2 boxCenter = (Vector2)transform.position + direction * depth / 2;
        Vector2 halfExtents = new Vector2(width / 2, depth / 2);
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(boxCenter, halfExtents, angle, layerMask);
        
        foreach (Collider2D hit in hitColliders)
        {
            hit.gameObject.GetComponent<Enemy>().TakeDamage(damage);
        }

        if (playerHealth.CheckIfFullHealth()){
            ThrowSword();
        }
    }

    private void ThrowSword()
    {

    }
}
