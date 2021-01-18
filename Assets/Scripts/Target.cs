using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Target : MonoBehaviour {
    
    public static int kills = 0;
    public GameObject canvas;
    public GameObject player;
    public float health = 100.0f;
    public bool respawn = false;

    public void TakeDamage(float damage) {
        health -= damage;
        if(health <= 0.0f) {
            Die();
        }
    }

    private void Die() {
        SetEnabled(false);
        if(respawn) {
            StartCoroutine(Respawn());
        } else {
            kills++;
            canvas.transform.Find("Text").GetComponent<Text>().text = "Kills: " + kills + "/10";
            if(kills == 10) {
                player.GetComponent<Inventory>().enabled = false;
                canvas.transform.Find("End").gameObject.SetActive(true);
            }
        }
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
        gameObject.GetComponent<Rigidbody>().isKinematic = !val;
        gameObject.GetComponent<BoxCollider>().enabled = val;
        
    }

}
