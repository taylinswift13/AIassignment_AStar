using UnityEngine;

public class StateMachine
{
    private State currentState;
    private Entity Parent;
    private bool isSwitchingState = false;

    public void SwitchState(State newState)
    {
        isSwitchingState = true;
        if (currentState != null)
        {
            currentState.Exit();
        }
        currentState = newState;
        newState.parent = Parent;
        newState.tileManager = Parent.tileManager;
        currentState.Enter();
        isSwitchingState = false;
    }
    public void Update(float deltaTime)
    {
        if (currentState != null && !isSwitchingState)
        {
            currentState.Update(deltaTime);
        }
    }

    public StateMachine(Entity parent)
    {
        this.Parent = parent;
    }
}
