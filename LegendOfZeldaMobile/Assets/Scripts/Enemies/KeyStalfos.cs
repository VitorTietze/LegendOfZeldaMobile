using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyStalfos : Stalfos
{
    protected override void Die()
    {
        GameObject key = Resources.Load<GameObject>("Prefabs/Items/Key");
        Instantiate(key, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
