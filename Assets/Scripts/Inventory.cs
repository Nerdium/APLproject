using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour, IPointerClickHandler{
    
    private GameObject inventory;
    private Canvas canvas;
    private Transform itemHolder;

    [SerializeField]
    private int gridWidth = 3;
    [SerializeField]
    private int gridHeight = 4;

    private List<ItemIcon> items;
    private int[,] gridIndices;

    private Slot primarySlot, secondarySlot;

    public Texture[] itemTextures = new Texture[Enum.GetValues(typeof(Item)).Length];

    private void Start() {
        inventory = transform.Find("Inventory").gameObject;
        canvas = inventory.GetComponent<Canvas>();
        itemHolder = inventory.transform.Find("Items");

        items = new List<ItemIcon>();
        gridIndices = new int[gridWidth, gridHeight];
        
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Tab)) {
            canvas.enabled = !canvas.enabled;
            GetComponent<RigidbodyFirstPersonController>().mouseLook.SetCursorLock(!canvas.enabled);
            GetComponent<RigidbodyFirstPersonController>().enabled = !canvas.enabled;
        }
    }

    

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Pickup") {
            AddToInventory(other.gameObject);
        }
    }

    //Adds an item to inventory if possible
    private void AddToInventory(GameObject obj) {
      Pickup pickup = obj.GetComponent<Pickup>();
      int[] size = ItemSize.Get(pickup.type);
      int[] pos = GetOpenPosition(size);
      if(pos[0] == -1) {
        return;
      }
      print(size[0] + ", " + size[1]);
      int[] guiVals = new int[] {pos[0] * 100, pos[1] * 100, size[0] * 100, size[1] * 100};

      ItemIcon objIcon = ItemIcon.Create(itemHolder, obj, guiVals, itemTextures[(int)(pickup.type)], ItemName.Get(pickup.type));
      //objIcon.gameObject.transform.parent = transform;

      items.Add(objIcon);
      AddToGrid(items.Count, pos, size);
    }

    //Finds the first position where rect of the given size is empty
    //Returns [-1, -1] if not possible position
    private int[] GetOpenPosition(int[] size) {
      for(int i = 0; i < gridHeight - size[1] + 1; i++) {
        for(int j = 0; j < gridWidth - size[0] + 1; j++) {
          int[] pos  = new int[] {j, i};
          if(isRectEmpty(pos, size)) {
            return pos;
          }
        }
      }
      return new int[] {-1, -1};
    }

    //Checks if grid is empty in rectangle
    private bool isRectEmpty(int[] pos, int[] size) {
      bool open = true;
      for(int w = 0; w < size[0]; w++) {
        for(int h = 0; h < size[1]; h++) {
          if(gridIndices[pos[0] + w, pos[1] + h] != 0) {
            open = false;
          }
        }
      }
      return open;
    }

    //Updates gridIndices with new index
    private void AddToGrid(int itemIndex, int[] pos, int[] size) {
      for(int i = pos[0]; i < pos[0] + size[0]; i++) {
        for(int j = pos[1]; j < pos[1] + size[1]; j++) {
          gridIndices[i, j] = itemIndex;
        } 
      }
    }

    public void OnPointerClick(PointerEventData eventData) {
        Debug.Log("Clicked: " + eventData.pointerCurrentRaycast.gameObject.name);
        
    }
}
