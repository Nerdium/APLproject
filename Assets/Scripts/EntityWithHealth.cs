using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityWithHealth : MonoBehaviour {
    
    public int health {get; private set;}
    public int maxHealth {get; private set;}

    public int Heal(int amount) {
        health = Mathf.Min(health + amount, maxHealth);
        return health;
    }

    public int TakeDamage(int amount) {
        health = Mathf.Max(health - amount, 0);
        return health;
    }

}
