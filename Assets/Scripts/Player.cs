using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : EntityWithHealth {

    public float maxHunger, maxThirst;
    public float hungerDecreaseRate, thirstDecreaseRate;
    private float hunger, thirst;

    public void Start() {
        hunger = maxHunger;
        thirst = maxThirst;
    }

    public void Update() {
        

        if(hunger <= 0 || thirst <= 0) {
        } else {
            hunger -= hungerDecreaseRate * Time.deltaTime;
            thirst -= thirstDecreaseRate * Time.deltaTime;
        }
    }


}
