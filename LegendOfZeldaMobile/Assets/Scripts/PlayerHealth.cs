using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    private PlayerMovement playerMovement;
    public static Transform player;

    private int maxHealth = 3;
    private float health;
    public bool isFullHealth => health == maxHealth;
    private bool immune;
    private bool touchingEnemy;

    private int keys;

    private GameObject emptyHeart;
    private GameObject fullHeart;
    private GameObject halfHeart;
    [SerializeField] private Transform emptyHearts;
    [SerializeField] private Transform filledHearts;
    
    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        player = transform;

        emptyHeart = Resources.Load<GameObject>("Prefabs/Hearts/EmptyHeart");
        fullHeart = Resources.Load<GameObject>("Prefabs/Hearts/FullHeart");
        halfHeart = Resources.Load<GameObject>("Prefabs/Hearts/HalfHeart");

        health = maxHealth;
        UpdateHearts();
    }

    private void TakeDamage(float damage)
    {
        health = Mathf.Max(health - damage, 0);
        UpdateHearts();
        if (health == 0) Die();
        else StartCoroutine(ImmunityTime());
    }

    private void Die()
    {
        // animation? + restart game
    }

    private void UpdateHearts()
    {
        foreach (Transform child in emptyHearts) Destroy(child.gameObject);
        foreach (Transform child in filledHearts) Destroy(child.gameObject);

        for (int i = 0; i < maxHealth; i++)
        {
            Instantiate(emptyHeart, emptyHearts);
        }

        for (int i = 0; i < Mathf.FloorToInt(health); i++)
        {
            Instantiate(fullHeart, filledHearts);
        }

        if (health % 1 != 0){
            Instantiate(halfHeart, filledHearts);
        }
    }

    private IEnumerator ImmunityTime()
    {
        immune = true;
        yield return new WaitForSeconds(3f);
        immune = false;

        if (touchingEnemy){
            TakeDamage(enemyTransform.GetComponent<Enemy>().damage);

            Vector2 knockbackDirection = (transform.position - enemyTransform.position).normalized;
            if (Mathf.Abs(knockbackDirection.x) > Mathf.Abs(knockbackDirection.y)){
                knockbackDirection = new Vector2(Mathf.Sign(knockbackDirection.x), 0);
            } else {
                knockbackDirection = new Vector2(0, Mathf.Sign(knockbackDirection.y));
            }
            StartCoroutine(playerMovement.GetKnockedBack(knockbackDirection));
        }
    }

    [SerializeField] private TextMeshProUGUI keysTMP;
    public void ChangeKeyAmount(int increment)
    {
        keys += increment;
        keysTMP.text = keys.ToString();
    }

    private Transform enemyTransform;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent<Enemy>(out Enemy enemy)){
            enemyTransform = other.transform;
            touchingEnemy = true;
            if (!immune){
                TakeDamage(enemy.damage);

                Vector2 knockbackDirection = (transform.position - other.transform.position).normalized;
                if (Mathf.Abs(knockbackDirection.x) > Mathf.Abs(knockbackDirection.y)){
                    knockbackDirection = new Vector2(Mathf.Sign(knockbackDirection.x), 0);
                } else {
                    knockbackDirection = new Vector2(0, Mathf.Sign(knockbackDirection.y));
                }
                StartCoroutine(playerMovement.GetKnockedBack(knockbackDirection));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        touchingEnemy = false;
    }
}
