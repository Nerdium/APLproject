using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using System;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {
     
    private GameObject inventory;
    private Canvas canvas;

    [SerializeField]
    private int gridWidth = 4;
    [SerializeField]
    private int gridHeight = 3;

    private List<ItemIcon> gridItems = new List<ItemIcon>();
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
        int[] size = ItemSize.Get(type);
        GetOpenPosition(size);
    }
    
    private void UpdateItemIcons(GameObject obj) {
        ItemIcon icon = new ItemIcon();
        icon.image = new GameObject();
        icon.image.AddComponent(typeof(RawImage));
        icon.image.texture = itemTextures[(int)type];
        icon.image.rectTransform
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Pickup") {
            Add(other.gameObject);
        }
    }

    private int[] GetOpenPosition(int[] size) {
        //print(gridWidth - size[0]);
        //print(gridHeight - size[1]);
        for(int i = 0; i < gridWidth - size[0]; i++) {
            for(int j = 0; j < gridHeight - size[1]; j++) {
                print(i + ", " + j);
            }
        }
        return new int[] {0, 0};
    }
}
