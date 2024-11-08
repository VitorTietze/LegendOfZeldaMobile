using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triforce : MonoBehaviour
{
    private void GetPicked()
    {
        GameManager.instance.FinalScreen();
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
