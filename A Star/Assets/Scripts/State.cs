public class State
{
    public Entity parent;
    public TileManager tileManager;

    public virtual void Enter()
    {
        Decide();
    }

    public void Update(float deltaTime)
    {
        Decide();
        Move(deltaTime);
    }
    public virtual void Exit()
    {

    }
    public virtual void Decide()
    {

    }
    public virtual void Move(float deltaTime)
    {

    }
}
