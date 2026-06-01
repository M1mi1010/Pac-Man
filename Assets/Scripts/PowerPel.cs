using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPel : Pellet //Inherit from pellet
{
    public float duration = 8.0f;

    protected override void Eat() 
    {
        //Multiplier when you eat ghosts, 
        FindObjectOfType<GameManager>().PowerPelletEaten(this);
    }
}
