using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemName {
    private static readonly string[] names = new string[] {
        "Empty",
        "Food",
        "Water",
        "Pistol",
        "Rifle",
        "Shotgun"
    };

    public static string Get(Item item) {
        return names[(int)item];
    }
    
}
