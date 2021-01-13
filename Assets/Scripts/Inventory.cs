using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using System;

public class Inventory : MonoBehaviour {
     
    private GameObject inventory;
    private Canvas canvas;

    [SerializeField]
    private int gridWidth = 4;
    [SerializeField]
    private int gridHeight = 3;

    private List<GameObject> gridItems = new List<GameObject>();
    private int[] itemIndices;

    private Slot primarySlot, secondarySlot;

    public Texture[] itemTextures = new Texture[Enum.GetValues(typeof(Item)).Length];

    private void Start() {
        inventory = transform.Find("Inventory").gameObject;
        canvas = inventory.GetComponent<Canvas>();

        itemIndices = new int[gridWidth * gridHeight];
        
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Tab)) {
            canvas.enabled = !canvas.enabled;
            GetComponent<RigidbodyFirstPersonController>().mouseLook.SetCursorLock(!canvas.enabled);
            GetComponent<RigidbodyFirstPersonController>().enabled = !canvas.enabled;
        }
    }

    private void Add(GameObject obj) {
        Item type = obj.GetComponent<Pickup>().type;
        print(itemTextures[(int)type]);
        print(ItemSize.Get(type));
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Pickup") {
            Add(other.gameObject);
        }
    }
}
