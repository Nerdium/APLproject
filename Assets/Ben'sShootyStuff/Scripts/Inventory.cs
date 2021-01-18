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

    private ItemIcon equipped;
    public GameObject decalPrefab;
    
    public float rifleFireRate = 15.0f;
    public float shotgunFireRate = 0.25f;
    public float gunVariance = 0.1f;
    private float nextTimeToFireRifle;
    private float nextTimeToFireShotgun;

    public Camera camera;
    public Transform hand;

    public Texture[] itemTextures = new Texture[Enum.GetValues(typeof(Item)).Length];
    public GameObject[] itemPickups = new GameObject[Enum.GetValues(typeof(Item)).Length];

    private int nextIndex = 1;

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
            Cursor.lockState = canvas.enabled ? CursorLockMode.None : CursorLockMode.Locked; 
            Cursor.visible = canvas.enabled;
            GetComponent<FirstPersonAIO>().fps_Rigidbody.velocity = Vector3.zero;
            GetComponent<FirstPersonAIO>().enabled = !canvas.enabled;

            nextTimeToFireRifle = Time.time;
            nextTimeToFireShotgun = Time.time;
        }

        if(equipped) {
          switch(equipped.item) {
            case Item.RIFLE:
              if(Input.GetButton("Fire1") && Time.time > nextTimeToFireRifle) {
                nextTimeToFireRifle = Time.time + 1 / rifleFireRate;
                hand.GetChild(0).GetComponent<Gun>().Shoot(1, gunVariance * 6);
            
              }
              break;
            
            case Item.PISTOL:
              if(Input.GetButtonDown("Fire1")) {
                hand.GetChild(0).GetComponent<Gun>().Shoot(1, gunVariance);
            
              }
            break;

            case Item.SHOTGUN:
              if(Input.GetButtonDown("Fire1") && Time.time > nextTimeToFireShotgun) {
                nextTimeToFireShotgun = Time.time + 1 / shotgunFireRate;
                hand.GetChild(0).GetComponent<Gun>().Shoot(20, gunVariance * 30);
            
              }
              break;
            
          }
        }

    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Pickup") {
            AddToInventory(other.gameObject);
        }
    }

    //Adds an item to inventory if possible
    private bool AddToInventory(GameObject obj2) {
      Pickup pickup = obj2.GetComponent<Pickup>();
      int[] size = ItemSize.Get(pickup.type);
      int[] pos = GetOpenPosition(size);
      if(pos[0] == -1) {
        return false;
      }
      print(size[0] + ", " + size[1]);
      int[] guiVals = new int[] {pos[0] * 100, pos[1] * 100, size[0] * 100, size[1] * 100};

      ItemIcon objIcon = ItemIcon.Create(itemHolder, obj2, guiVals, itemTextures[(int)(pickup.type)], ItemName.Get(pickup.type), nextIndex);
      //objIcon.gameObject.transform.parent = transform;
            
            if(equipped) {
              MoveOrDrop(equipped); 
              Destroy(hand.transform.GetChild(0).gameObject);
            }

            equipped = objIcon;
            GameObject obj = Instantiate(itemPickups[(int)(objIcon.item)], hand);
            obj.GetComponent<Rigidbody>().isKinematic = true;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
            if(objIcon.item == Item.PISTOL || objIcon.item == Item.SHOTGUN || objIcon.item == Item.RIFLE) {
              Gun gun = obj.AddComponent<Gun>();
              gun.decalPrefab = decalPrefab;
            }
            BoxCollider[] colliders = obj.GetComponents<BoxCollider>();
            foreach(BoxCollider collider in colliders) {
              collider.enabled = false;
            }
            RemoveFromGrid(objIcon.index);
            objIcon.gameObject.transform.SetParent(inventory.transform.Find("ActiveItemHolder"));
            objIcon.transform.localPosition = new Vector3(-50.0f, 50.0f, 0.0f);
      return true;
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

    private void RemoveFromGrid(int index) {
      for(int i = 0; i < gridWidth; i++) {
        for(int j = 0; j < gridHeight; j++) {
          if(gridIndices[i, j] == index) {
            gridIndices[i, j] = 0;
          }
        }
      }
    }

    private ItemIcon GetFromIndex(int index) {
      foreach(ItemIcon item in items) {
        if(item.index == index) {
          return item;
        }
      }
      return null;
    }

    private void DropItem(ItemIcon item) {
      RemoveFromGrid(item.index);
      items.Remove(item);

      Vector3 front = transform.position + camera.transform.forward * 10.0f;
      GameObject newPickup = Instantiate(itemPickups[(int)(item.item)], new Vector3(front.x, 10.0f, front.z), Quaternion.identity);
      Destroy(item.gameObject);
    }

    private void MoveOrDrop(ItemIcon item) {
      int[] size = ItemSize.Get(item.item);
      int[] pos = GetOpenPosition(size);

      if(pos[0] == -1) {
        print("dropping");
        DropItem(item);
        equipped = null;
        Destroy(hand.transform.GetChild(0));
      } else {
        print("moving");
        int[] guiVals = new int[] {pos[0] * 100, pos[1] * 100, size[0] * 100, size[1] * 100};
        ItemIcon.SetTransform(item.gameObject, itemHolder, guiVals);
        items.Add(item);
        AddToGrid(nextIndex, pos, size);
        nextIndex++;
      }
    }

    public void OnPointerClick(PointerEventData eventData) {
      ItemIcon item = eventData.pointerCurrentRaycast.gameObject.GetComponent<ItemIcon>();
      print(item);
      if(item) {
        if(eventData.button == PointerEventData.InputButton.Right) {
          DropItem(item);
          print(item);

        } else {
          print(equipped);
          if(item == equipped) {
            DropItem(item);
            equipped = null;
            print(hand.transform.GetChild(0).gameObject);
            Destroy(hand.transform.GetChild(0).gameObject);
          } else {

            if(equipped) {
              MoveOrDrop(equipped); 
            }


            equipped = item;
            GameObject obj = Instantiate(itemPickups[(int)(item.item)], hand);
            obj.GetComponent<Rigidbody>().isKinematic = true;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
            if(item.item == Item.PISTOL || item.item == Item.SHOTGUN || item.item == Item.RIFLE) {
              Gun gun = obj.AddComponent<Gun>();
              gun.decalPrefab = decalPrefab;
            }
            BoxCollider[] colliders = obj.GetComponents<BoxCollider>();
            foreach(BoxCollider collider in colliders) {
              collider.enabled = false;
            }
            RemoveFromGrid(item.index);
            item.gameObject.transform.SetParent(inventory.transform.Find("ActiveItemHolder"));
            item.transform.localPosition = new Vector3(-50.0f, 50.0f, 0.0f);
          }
          
          
        }
      }
    }
}
