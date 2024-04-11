using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarChaserState_Trade : State
{
    private StarChaser parent;
    private TradePost tradePost;
    private List<Tile> path;
    private float timer = 0.0f;
    private int nextPos;
    private bool arrived = false;
    public override void Enter()
    {
        parent = (StarChaser)base.parent;
        parent.LineCleaner();
        nextPos = 0;
        GetNearestTradePost();
        parent.PrintPath(path, nextPos);
        base.Enter();
    }

    public override void Decide()
    {
        if (arrived == true && tradePost != null)
        {
            parent.collectedStars++;
            parent.SwitchState(new StarChaserState_Find());
            return;
        }
        if (parent.stamina <= 0)
        {
            DropStar();
            Debug.Log("Drop carrying star");
            parent.SwitchState(new StarChaserState_Rest());
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
        parent = null;
        tradePost = null;
        path = null;
    }
    private void GetNearestTradePost()
    {
        float nearestDistance = 1000;

        List<Entity> entities = tileManager.GetAllEntitiesOfType("TradePost");

        foreach (Entity entity in entities)
        {
            float distance = Vector2.Distance(parent.gameObject.transform.position, entity.gameObject.transform.position);

            if (distance < nearestDistance)
            {
                tradePost = (TradePost)entity;
                nearestDistance = distance;
            }
        }
        if (tradePost != null)//same tile
        {
            if (tradePost.transform.position == parent.gameObject.transform.position)
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
            if (tradePost == null)
                Debug.Log("Can't find Trade Post");
            if (tradePost != null)
                Debug.Log("Nearest TradePost is blocked");
            return;
        }
    }

    private void GetPath()
    {
        Tile curTile = tileManager.GetTile(parent.gameObject);
        Tile tradeTile = tileManager.GetTile(tradePost.gameObject);
        path = tileManager.pathFinder.GetPath(curTile, tradeTile);
    }
    private void DropStar()
    {
        if (tileManager.GetObjectsInTile(tileManager.GetTile(parent.gameObject)).Count != 0)
            tileManager.AddObject(parent.starPrefab, parent.transform.position + Vector3.one);
    }

}
