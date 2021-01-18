using UnityEngine;
using System.Collections;

public class Target : MonoBehaviour {
    
    public float health = 100.0f;

    public void TakeDamage(float damage) {
        health -= damage;
        if(health <= 0.0f) {
            Die();
        }
    }

    private void Die() {
        SetEnabled(false);
        StartCoroutine(Respawn());
    }

    IEnumerator Respawn() {
        yield return new WaitForSeconds(5.0f);
        SetEnabled(true);
        health = 100.0f;
    }

    void SetEnabled(bool val) {
        for(int i = 0; i < transform.childCount; i++) {
            transform.GetChild(i).gameObject.SetActive(val);
        }
        gameObject.GetComponent<BoxCollider>().enabled = val;
    }

}
