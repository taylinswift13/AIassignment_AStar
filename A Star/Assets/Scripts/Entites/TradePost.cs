using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradePost : Entity
{
    public static TradePost instance;

    protected override void Init()
    {
        instance = this;
        Type = "TradePost";
    }

    public Vector2 GetAxis()
    {
        return tileManager.GetAxis(gameObject);
    }

}
