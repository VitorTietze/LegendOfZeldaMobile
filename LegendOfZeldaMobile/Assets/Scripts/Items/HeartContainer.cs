using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartContainer : MonoBehaviour
{
    private void GetPicked()
    {
        PlayerHealth.player.GetComponent<PlayerHealth>().GainHeart(1);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GetPicked();
        }
    }
}
