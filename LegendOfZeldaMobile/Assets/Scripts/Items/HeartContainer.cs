using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartContainer : Item
{
    protected override void GetPicked()
    {
        PlayerHealth.player.GetComponent<PlayerHealth>().GainHeart(1);
        Destroy(gameObject);
    }
}
