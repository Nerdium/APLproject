using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gun : MonoBehaviour {

    public float damage = 10.0f;
    //public float range = 100.0f;
    public float fireRate = 100.0f;
    public float impactForce = 10.0f;
    //public float variance = 0.1f;
    public float muzzleVeloctiy = 100.0f;

    public GameObject decalPrefab;

    //private float nextTimeToFire = 0.0f;

    private Camera fpsCam;   
    private LineRenderer laserLine;

    private Item active;

    void Start() {
        //laserLine = GetComponent<LineRenderer>();
        fpsCam = GetComponentInParent<Camera>();

        //laserLine.startWidth = 0.1f;
        //laserLine.enabled = true;

        
    }

    void Update() {
        
    }

    public void Shoot(int amount, float variance) {
        for(int i = 0; i < amount; i++) {
            float range = 100.0f;

            Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

            float[] bulletSpread = GetSpread(variance);

            Vector3 direction = Quaternion.Euler(bulletSpread[0], bulletSpread[1], 0.0f) * fpsCam.transform.forward;
        
        
            List<Vector3> points = new List<Vector3>();
            points.Add(rayOrigin);

            RaycastHit hit;
            if(Physics.Raycast(rayOrigin, direction, out hit)) {
                
                    GameObject decalObject = Instantiate(decalPrefab, hit.point + (hit.normal * 0.0025f), Quaternion.FromToRotation(Vector3.up, hit.normal));
                    decalObject.transform.SetParent(hit.transform, true);
                    Object.Destroy(decalObject, 5.0f);
                    if(hit.rigidbody) {
                        hit.rigidbody.AddForce(-hit.normal * impactForce);
                    }
                    Target target = hit.collider.gameObject.GetComponent<Target>();
                    if(target) {
                        target.TakeDamage(10);
                    }
                }

        }
    }

    public static float[] GetSpread(float variance) {
        float s = 0.0f;
        float u = 0.0f;
        float v = 0.0f;
        while(s == 0.0f || s >= 1.0f) {
            u = 2.0f * Random.Range(0f,1f) - 1.0f;
            v = 2.0f * Random.Range(0f,1f) - 1.0f;
            s = u*u + v*v;
        }

        s = Mathf.Sqrt(-2 * Mathf.Log(s) / s);
        u = u * s * variance;
        v = v * s * variance;

        return new float[] {u, v};
    }
}
