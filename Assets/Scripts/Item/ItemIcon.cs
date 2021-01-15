using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemIcon : MonoBehaviour {
    
    public GameObject item;

    public static ItemIcon Create(Transform parent, GameObject item, int[] guiVals, Texture texture, string name) {
        GameObject obj = new GameObject(name);
        obj.transform.parent = parent;
        ItemIcon itemIcon = obj.AddComponent<ItemIcon>();

        itemIcon.item = item;
        item.transform.parent = obj.transform;
        item.SetActive(false);

        RawImage image = obj.AddComponent<RawImage>();
        image.texture = texture;
        
        RectTransform rectTransform = obj.GetComponent<RectTransform>();
        rectTransform.rotation = parent.rotation;
        rectTransform.localScale = new Vector3(1.0f, 1.0f, 0.0f);

        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, guiVals[2]);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, guiVals[3]);

        rectTransform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        rectTransform.anchorMin = new Vector2(0.0f, 1.0f);
        rectTransform.anchorMax = new Vector2(0.0f, 1.0f);
        rectTransform.pivot = new Vector2(0.0f, 1.0f);
        
        rectTransform.anchoredPosition = new Vector3(guiVals[0], -guiVals[1], 0.0f);


        return itemIcon;
    }

    private void Update() {
        //print(gameObject.GetComponent<RectTransform>().rotation);
    }
}
