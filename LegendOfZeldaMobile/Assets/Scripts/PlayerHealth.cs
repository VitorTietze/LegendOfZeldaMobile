using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private int maxHealth = 3;
    private float health;
    public bool isFullHealth => health == maxHealth;

    private GameObject emptyHeart;
    private GameObject fullHeart;
    private GameObject halfHeart;
    [SerializeField] private Transform emptyHearts;
    [SerializeField] private Transform filledHearts;
    
    private void Awake()
    {
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
    }

    private void GetKnockedBack()
    {

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

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent<Enemy>(out Enemy enemy)){
            TakeDamage(enemy.damage);
            GetKnockedBack();
        }
    }
}
