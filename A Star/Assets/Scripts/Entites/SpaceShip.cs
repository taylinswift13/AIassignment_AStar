using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShip : Entity
{
    public static SpaceShip instance;
    protected override void Init()
    {
        instance = this;
        Type = "SpaceShip";
    }
    public Vector2 GetAxis()
    {
        return tileManager.GetAxis(gameObject);
    }
}
