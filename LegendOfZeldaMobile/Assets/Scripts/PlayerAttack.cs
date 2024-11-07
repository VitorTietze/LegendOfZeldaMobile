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
    private float width = 1.5f;
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
        direction = new Vector2(playerMovement.horizontal, playerMovement.vertical);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Vector2 boxCenter = (Vector2)transform.position + direction * depth / 2;
        Vector2 halfExtents = new Vector2(width / 2, depth / 2);
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(boxCenter, halfExtents, angle, layerMask);
        DrawBox(angle, boxCenter, halfExtents);
        
        foreach (Collider2D hit in hitColliders)
        {
            hit.gameObject.GetComponent<Enemy>()?.TakeDamage(damage);
        }

        if (playerHealth.isFullHealth){
            ThrowSword();
        }
    }

    private void DrawBox(float angle, Vector2 boxCenter, Vector2 halfExtents)
    {
        Vector2 right = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
        Vector2 up = new Vector2(-right.y, right.x);

        Vector2 topRight = boxCenter + right * halfExtents.x + up * halfExtents.y;
        Vector2 topLeft = boxCenter - right * halfExtents.x + up * halfExtents.y;
        Vector2 bottomRight = boxCenter + right * halfExtents.x - up * halfExtents.y;
        Vector2 bottomLeft = boxCenter - right * halfExtents.x - up * halfExtents.y;

        Debug.DrawLine(topLeft, topRight, Color.red, 0.1f);
        Debug.DrawLine(topRight, bottomRight, Color.red, 0.1f);
        Debug.DrawLine(bottomRight, bottomLeft, Color.red, 0.1f);
        Debug.DrawLine(bottomLeft, topLeft, Color.red, 0.1f);
    }

    private void ThrowSword()
    {
        GameObject swordObj = Instantiate(thrownSword, transform.position, Quaternion.identity, transform);
        swordObj.GetComponent<ThrownSword>().Initialize(direction, damage);

        /* GameObject boomerang = Resources.Load<GameObject>("Prefabs/EnemyBoomerang");
        GameObject boomObj = Instantiate(boomerang, transform.position, Quaternion.identity, transform);
        boomObj.GetComponent<EnemyBoomerang>().Initialize(direction, transform); */
    }
}
