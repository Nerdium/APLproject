using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemIcon : MonoBehaviour {
    
    public Item item;
    public int index;

    public static ItemIcon Create(Transform parent, GameObject itemObj, int[] guiVals, Texture texture, string name, int index) {
        GameObject obj = new GameObject(name);
        ItemIcon itemIcon = obj.AddComponent<ItemIcon>();

        itemIcon.index = index;

        itemIcon.item = itemObj.GetComponent<Pickup>().type;
        Object.Destroy(itemObj);

        RawImage image = obj.AddComponent<RawImage>();
        image.texture = texture;

        SetTransform(obj, parent, guiVals);


        return itemIcon;
    }

    public static void SetTransform(GameObject icon, Transform parent, int[] guiVals) {
        icon.transform.SetParent(parent);

        RectTransform rectTransform = icon.GetComponent<RectTransform>();
        rectTransform.rotation = icon.transform.parent.rotation;
        rectTransform.localScale = new Vector3(1.0f, 1.0f, 0.0f);

        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, guiVals[2]);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, guiVals[3]);

        rectTransform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        rectTransform.anchorMin = new Vector2(0.0f, 1.0f);
        rectTransform.anchorMax = new Vector2(0.0f, 1.0f);
        rectTransform.pivot = new Vector2(0.0f, 1.0f);
        
        rectTransform.anchoredPosition = new Vector3(guiVals[0], -guiVals[1], 0.0f);
    }

    private void Update() {
        //print(gameObject.GetComponent<RectTransform>().rotation);
    }
}
