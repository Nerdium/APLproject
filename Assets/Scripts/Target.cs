using UnityEngine;

public class Target : MonoBehaviour {
    
    public float health = 50.0f;

    public void TakeDamage(float damage) {
        health -= damage;
        if(health <= 0.0f) {
            Die();
        }
    }

    private void Die() {
        Destroy(gameObject);
    }

}
