using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSize {
    private readonly int[,] sizes = new int[,] {
        {1, 1}, //Empty
        {1, 1}, //Food
        {1, 1}, //Water
        {2, 1}, //Pistol
        {3, 1}, //Rifle
        {3, 1} //Shotgun
    };

    public int[] Get(Item item) {
        return sizes[(int)item];
    }
    
}
