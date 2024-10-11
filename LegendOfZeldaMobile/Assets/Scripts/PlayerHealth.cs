using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private int maxHealth = 3;
    private float health = 3f;
    
    public bool CheckIfFullHealth()
    {
        return health == maxHealth;
    }
}
