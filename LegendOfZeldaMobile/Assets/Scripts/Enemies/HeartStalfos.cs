using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartStalfos : Stalfos
{
    protected override void Die()
    {
        GameObject heart = Resources.Load<GameObject>("Prefabs/Items/Heart");
        Instantiate(heart, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
