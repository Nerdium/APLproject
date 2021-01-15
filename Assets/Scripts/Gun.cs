using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {

    public float damage = 10.0f;
    public float range = 100.0f;
    public float fireRate = 15.0f;
    public float impactForce = 1000.0f;

    public ParticleSystem muzzleFlash;
    public Transform gunEnd;

    private float nextTimeToFire = 0.0f;

    private Camera fpsCam;   
    private LineRenderer laserLine;

    void Start() {
        laserLine = GetComponent<LineRenderer>();
        fpsCam = GetComponentInParent<Camera>();
    }

    void Update() {
        if(Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire) {
            nextTimeToFire = Time.time + 1.0f / fireRate;
            Shoot();
        }
    }

    void Shoot() {
        Vector3 rayOrigin = fpsCam.ViewportToWorldPoint (new Vector3(0.5f, 0.5f, 0.0f));
        muzzleFlash.Play();
        StartCoroutine(ShotEffect());
        RaycastHit hit;
        if(Physics.Raycast(rayOrigin, fpsCam.transform.forward, out hit, range)) {
            Debug.DrawRay(transform.position, transform.forward, Color.green); print("Hit");
            Debug.Log(hit.transform.name);
            laserLine.SetPosition (0, gunEnd.position);
            laserLine.SetPosition (1, hit.point);

            Target target = hit.transform.GetComponent<Target>();
            if(target) {
                target.TakeDamage(damage);
            }

            if(hit.rigidbody) {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }
        }
    }

    private IEnumerator ShotEffect() {

        // Turn on our line renderer
        laserLine.enabled = true;

        //Wait for .07 seconds
        yield return 0.5f;

        // Deactivate our line renderer after waiting
        laserLine.enabled = false;
    }
}
