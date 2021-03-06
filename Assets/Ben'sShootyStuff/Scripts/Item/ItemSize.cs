﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSize {
    private static readonly int[][] sizes = new int[][] {
        new int[] {1, 1}, //Ammo
        new int[] {1, 1}, //Food
        new int[] {1, 2}, //Water
        new int[] {2, 1}, //Pistol
        new int[] {3, 1}, //Rifle
        new int[] {3, 1} //Shotgun
    };

    public static int[] Get(Item item) {
        return sizes[(int)item];
    }
    
}
