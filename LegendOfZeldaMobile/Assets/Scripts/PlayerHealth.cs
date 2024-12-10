using System;
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
    public /* static */ bool immune;
    [HideInInspector] public bool touchingEnemy;

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

    private void Start()
    {
        GameManager.instance.link = transform;
    }

    public void TakeDamage(float damage, bool triggerImmunity = true)
    {
        if (immune) return;
        Debug.Log("touchingEnemy: " + touchingEnemy);
        health = Mathf.Max(health - damage, 0);
        UpdateHearts();
        if (health == 0) Die();
        else if (triggerImmunity){
            StartCoroutine(ImmunityTime(1.5f));
        }
    }

    private void Die()
    {
        // animation? + restart game
        GameManager.instance.GameOver();
    }

    public void HealHeart(float amount)
    {
        if (amount % 0.5f != 0) throw new Exception("Amount to heal must be divisible by 0.5.");
        health = Mathf.Min(maxHealth, health + amount);
        UpdateHearts();
    }

    public void GainHeart(int amount)
    {
        maxHealth += amount;
        UpdateHearts();
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

    private IEnumerator ImmunityTime(float duration = 1f)
    {
        immune = true;
        yield return new WaitForSeconds(duration);
        immune = false;

        if (touchingEnemy){
            if (enemyTransform != null){
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
    }

    [SerializeField] private TextMeshProUGUI keysTMP;
    public void ChangeKeyAmount(int increment)
    {
        keys += increment;
        keysTMP.text = keys.ToString();
    }

    private void UnlockDoor(Transform doorToUnlock)
    {
        ChangeKeyAmount(-1);
        doorToUnlock.GetComponent<BoxCollider2D>().enabled = false;
        Destroy(doorToUnlock.Find("Sprite1").gameObject);
        Destroy(doorToUnlock.Find("Sprite2").gameObject);
    }

    private Transform enemyTransform;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.parent?.gameObject.TryGetComponent<Enemy>(out Enemy enemy) == true){
            enemyTransform = other.transform.parent;
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

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Doors"))
        {
            if (keys > 0 && other.otherCollider is CapsuleCollider2D){
                UnlockDoor(other.transform);
            }
        }
    }
}
