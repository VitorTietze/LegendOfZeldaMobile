using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;
using UnityEngine.UIElements;

public class PlayerAttack : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerHealth playerHealth;
    private GameObject thrownSword;
    private float damage = 3f; // arbitrary
    private float width = 1f;
    private float depth = 1f;
    private Vector2 direction;
    private LayerMask layerMask;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerHealth = GetComponent<PlayerHealth>();
        thrownSword = Resources.Load<GameObject>("Prefabs/ThrownSword");
        layerMask = LayerMask.NameToLayer("Enemies");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SwordAttack();
        }
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
            hit.gameObject.GetComponent<Enemy>()?.TakeDamage(damage);
        }

        if (playerHealth.isFullHealth){
            ThrowSword();
        }
    }

    private void ThrowSword()
    {
        GameObject swordObj= Instantiate(thrownSword, transform.position, Quaternion.identity, transform);
        swordObj.GetComponent<ThrownSword>().Initialize(direction, damage);
    }
}
