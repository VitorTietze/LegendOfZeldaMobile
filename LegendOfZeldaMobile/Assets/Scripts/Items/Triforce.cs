using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triforce : Item
{
    protected override void GetPicked()
    {
        GameManager.instance.FinalScreen();
        Destroy(gameObject);
    }
}
