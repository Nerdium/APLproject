using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    private int ammo = 5;
    public Text ammoText;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ammoText.text = "Ammo : " + ammo;
        if (Input.GetMouseDown(0))
        {
            ammo--;
        }
    }
}

