using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    protected StateMachine stateMachine;
    public TileManager tileManager;

    public string Type = "None";

    private void Awake()
    {
        stateMachine = new StateMachine(this);//Create the state machine
        tileManager = GameManager.instance.tileManager;
        Init();
    }

    protected void Update()
    {
        stateMachine.Update(Time.deltaTime);
    }

    protected virtual void Init()   //Entrance of all game entities
    {

    }

    public void SwitchState(State newState)
    {
        stateMachine.SwitchState(newState);
    }
}
