using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : Item
{
    protected override void GetPicked()
    {
        PlayerHealth.player.GetComponent<PlayerHealth>().HealHeart(1f);
        Destroy(gameObject);
    }
}
