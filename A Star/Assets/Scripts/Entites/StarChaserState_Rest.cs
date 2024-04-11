using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarChaserState_Rest : State
{
    private StarChaser parent;
    private SpaceShip spaceShip;
    private List<Tile> path;
    private float timer = 0.0f;
    private int nextPos;
    private bool arrived = false;
    public override void Enter()
    {
        parent = (StarChaser)base.parent;
        parent.LineCleaner();
        parent.stamina = 1000;         //Don't need to count stamina while going back to space ship
        parent.carryingStar = false;
        nextPos = 0;
        GetNearestSpaceShip();
        parent.PrintPath(path, nextPos);
        base.Enter();
    }

    public override void Decide()
    {
        if (arrived == true)
        {
            parent.SwitchState(new StarChaserState_Find());
            return;
        }
    }
    public override void Move(float deltaTime)
    {
        if (path == null || path.Count == 0) return;

        if (nextPos == path.Count) //If reached the end
        {
            arrived = true;
            return;
        }
        else
        {
            timer += deltaTime;
            if (timer >= parent.moveFrequency && nextPos < path.Count)
            {
                tileManager.MoveToOneTile(parent.gameObject, path[nextPos].position);
                nextPos++;
                parent.stamina--;
                timer = 0.0f;
            }
        }
    }
    public override void Exit()
    {
        parent.stamina = parent.maxStamina;
        parent = null;
        spaceShip = null;
        path = null;
    }
    private void GetNearestSpaceShip()
    {
        float nearestDistance = 1000;

        List<Entity> entities = tileManager.GetAllEntitiesOfType("SpaceShip");

        foreach (Entity entity in entities)
        {
            float distance = Vector2.Distance(parent.gameObject.transform.position, entity.gameObject.transform.position);

            if (distance < nearestDistance)
            {
                spaceShip = (SpaceShip)entity;
                nearestDistance = distance;
            }
        }
        if (spaceShip != null)
        {
            if (spaceShip.transform.position == parent.gameObject.transform.position)
            {
                arrived = true;
                return;
            }
            else
            {
                GetPath();
            }
        }
        if (path == null || path.Count == 0)
        {
            if (spaceShip == null)
                Debug.Log("Can't find Space Ship");
            if (spaceShip != null)
                Debug.Log("Nearest Spaceship is blocked");
            return;
        }
    }
    private void GetPath()
    {
        Tile curTile = tileManager.GetTile(parent.gameObject);
        Tile targetTile = tileManager.GetTile(spaceShip.gameObject);
        path = tileManager.pathFinder.GetPath(curTile, targetTile);
    }
}
