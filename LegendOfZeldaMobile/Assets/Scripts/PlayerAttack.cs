using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;
using UnityEngine.UIElements;

public class PlayerAttack : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerHealth playerHealth;
    private AnimatedSpritePlus animatedPlus;
    private GameObject thrownSword;
    private float damage = 1f;
    public float attackTime = 0.25f;
    private float width = 1.5f;
    private float depth = 2.5f;//1.85f; // 2.5 might be it tho
    private Vector2 direction;
    private LayerMask layerMask;
    public bool isAttacking;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerHealth = GetComponent<PlayerHealth>();
        animatedPlus = transform.Find("Sprite").GetComponent<AnimatedSpritePlus>();

        thrownSword = Resources.Load<GameObject>("Prefabs/ThrownSword");
        layerMask = LayerMask.GetMask("Enemies", "FlyingEnemies");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)){
            SwordAttack();
        }
    }

    public void SwordAttack()
    {
        if (isAttacking) return;

        animatedPlus.attacking = true;
        isAttacking = true;
        StartCoroutine(StopAttacking());

        direction = new Vector2(playerMovement.horizontal, playerMovement.vertical);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Vector2 boxCenter = (Vector2)transform.position + direction * depth / 2;
        Vector2 halfExtents = new Vector2(depth / 2, width / 2);
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(boxCenter, halfExtents, angle, layerMask);
        DrawBox(angle, boxCenter, halfExtents);
        
        foreach (Collider2D hit in hitColliders)
        {
            if (hit.gameObject.TryGetComponent<Enemy>(out Enemy enemy)){
                enemy.TakeDamage(damage);
                StartCoroutine(enemy.GetKnockedBack(direction));
            }
        }

        if (playerHealth.isFullHealth){
            Invoke(nameof(ThrowSword), 0.25f);
        }
    }

    private IEnumerator StopAttacking()
    {
        yield return new WaitForSeconds(attackTime);
        animatedPlus.attacking = false;
        isAttacking = false;
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

    public void ThrowSword()
    {
        Vector2 adjustedPosition = (Vector2)transform.position + direction * 0.7f;
        GameObject swordObj = Instantiate(thrownSword, adjustedPosition, Quaternion.identity, transform);
        swordObj.GetComponent<ThrownSword>().Initialize(direction, damage);
    }
}
