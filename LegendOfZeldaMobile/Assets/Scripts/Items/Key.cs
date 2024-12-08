using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Item
{
    protected override void GetPicked()
    {
        PlayerHealth.player.GetComponent<PlayerHealth>().ChangeKeyAmount(1);
        Destroy(gameObject);
    }
}
