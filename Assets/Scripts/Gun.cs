using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gun : MonoBehaviour {

    public float damage = 10.0f;
    //public float range = 100.0f;
    public float fireRate = 100.0f;
    public float impactForce = 1000.0f;
    public float variance = 0.1f;
    public float muzzleVeloctiy = 100.0f;

    public ParticleSystem muzzleFlash;
    public Transform gunEnd;
    public GameObject decalPrefab;
    public GameObject decalHolder;

    private float nextTimeToFire = 0.0f;

    private Camera fpsCam;   
    private LineRenderer laserLine;

    private Item active;

    void Start() {
        laserLine = GetComponent<LineRenderer>();
        fpsCam = GetComponentInParent<Camera>();

        laserLine.startWidth = 0.1f;
        laserLine.enabled = true;

        
    }

    void Update() {
        if(Input.GetButton("Fire1") && Time.time > nextTimeToFire) {
            nextTimeToFire = Time.time + 1 / fireRate;
            Shoot();
            
        }
    }

    void Shoot() {
        muzzleFlash.Play();

        float range = 100.0f;

        Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

        float[] bulletSpread = GetSpread(variance);

        Vector3 direction = Quaternion.Euler(bulletSpread[0], bulletSpread[1], 0.0f) * fpsCam.transform.forward;
        
        
        List<Vector3> points = new List<Vector3>();
        points.Add(rayOrigin);
        bool isHit = false;

        //Quaternion.Euler(bulletSpread[0], bulletSpread[1], 0.0f) * 

        RaycastHit hit;
        if(Physics.Raycast(rayOrigin, direction, out hit, range)) {
                //print(hit.point);
                //isHit = true;
                
                GameObject decalObject = Instantiate(decalPrefab, hit.point + (hit.normal * 0.0025f), Quaternion.FromToRotation(Vector3.up, hit.normal));
                decalObject.transform.SetParent(hit.transform, true);
                Object.Destroy(decalObject, 5.0f);
                if(hit.rigidbody) {
                    hit.rigidbody.AddForce(-hit.normal * impactForce);
                }
            }


        int i = 1;
        while(!isHit && i < 100) {
            
            //points.Add(points[i - 1] + direction.normalized * range);
            


            i++;
        }
        //print(points.Count);
        laserLine.positionCount = points.Count;
        laserLine.SetPositions(points.ToArray());

        /*
        if(Physics.Raycast(rayOrigin, Quaternion.Euler(bulletSpread[0], bulletSpread[1], 0.0f) * fpsCam.transform.forward, out hit, range)) {
            //laserLine.SetPosition (0, gunEnd.position);
            //laserLine.SetPosition (1, hit.point);

            Target target = hit.transform.GetComponent<Target>();
            GameObject decalObject = Instantiate(decalPrefab, hit.point + (hit.normal * 0.025f), Quaternion.FromToRotation(Vector3.up, hit.normal), decalHolder.transform);
            if(target) {
                target.TakeDamage(damage);
            }

            
        }
        */
    }

    private void GetPosition(float totalDistance) {
        float t = totalDistance / muzzleVeloctiy;
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
