using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarChaserState_Find : State
{
    private StarChaser parent;
    private FallenStar star;
    private List<Tile> path = new List<Tile>();
    private float timer = 0.0f;
    private int nextPos;
    private bool arrived = false;
    public override void Enter()
    {
        parent = (StarChaser)base.parent;
        parent.LineCleaner();
        parent.carryingStar = false;
        nextPos = 0;
        GetNearestStar();
        parent.PrintPath(path, nextPos);
        base.Enter();
    }

    public override void Decide()
    {
        if (arrived == true && star != null)
        {
            tileManager.RemoveObject(star.gameObject);
            parent.carryingStar = true;
            parent.SwitchState(new StarChaserState_Trade());
            return;
        }
        if (parent.stamina <= 0)
        {
            parent.SwitchState(new StarChaserState_Rest());
            return;
        }
    }
    public override void Move(float deltaTime)
    {
        if (path == null) return;
        if (path.Count == 0) return;
        if (path != null && path.Count != 0)
        {
            if (nextPos == path.Count) //If reached the end
            {
                arrived = true;
            }
            else
            {
                timer += deltaTime;
                if (timer >= parent.moveFrequency)
                {
                    tileManager.MoveToOneTile(parent.gameObject, path[nextPos].position); //Move one step alone the path
                    nextPos++;
                    timer = 0.0f;
                    parent.stamina--;
                }
            }
        }

    }
    public override void Exit()
    {
        parent = null;
        star = null;
        path = null;
    }

    private void GetNearestStar()
    {
        float nearestDistance = 1000;

        List<Entity> entities = tileManager.GetAllEntitiesOfType("FallenStar");

        foreach (Entity entity in entities)
        {
            float distance = Vector2.Distance(parent.gameObject.transform.position, entity.gameObject.transform.position);

            if (distance < nearestDistance)
            {
                star = (FallenStar)entity;
                nearestDistance = distance;
            }
        }
        if (star != null)//same tile
        {
            if (star.transform.position == parent.gameObject.transform.position)
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
            if (star == null)
                Debug.Log("Can't find star");
            if (star != null)
                Debug.Log("Nearest star is blocked");
            return;
        }

    }

    private void GetPath()
    {
        Tile curTile = tileManager.GetTile(parent.gameObject); //current tile
        Tile starTile = tileManager.GetTile(star.gameObject); //star tile
        path = tileManager.pathFinder.GetPath(curTile, starTile);
    }
}
