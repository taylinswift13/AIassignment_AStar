using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    public static DebugManager instance;

    bool objectMove = false;
    Vector2 from;
    Vector2 to;
    bool create = false;
    Vector2 axis;
    string prefab = "Block";
    bool remove = false;
    Vector2 removeAxis;

    GameManager gameManager;
    TileManager tileManager;
    Tile selectedTile = null;
    GameObject frame;

    private void Start()
    {
        gameManager = GameManager.instance;
        tileManager = gameManager.tileManager;
        frame = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Frame"), Vector3.one, new Quaternion(0, 0, 0, 0));
        instance = this;
    }

    private void Update()
    {
        if (objectMove == true)
        {
            MoveObject();
            objectMove = false;
        }

        if (create == true)
        {
            CreateObject(axis, prefab);
            create = false;
        }

        if (remove == true)
        {
            RemoveObject(removeAxis);
            remove = false;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            SelectTile();
        }
    }

    private void SelectTile()
    {
        Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pz.z = 0;

        float minDis = 1000f;

        foreach (var tile in gameManager.tileManager.tiles)
        {
            float distance = Vector2.Distance(tile.position, pz);

            if (distance < minDis && distance <= 1)
            {
                minDis = distance;
                selectedTile = tile;
            }
        }

        if (selectedTile != null)
        {
            //  print(selectedTile.position);
            frame.GetComponent<SpriteRenderer>().enabled = true;
            frame.transform.position = selectedTile.position;
        }
    }
    public void MoveObject()
    {
        Tile tile = gameManager.tileManager.GetTile(from);
        GameObject target = tile.objects[0];

        RemoveObject(to);
        gameManager.tileManager.TeleportObject(tile, to, target);
    }

    public void RemoveObject(Vector2 axis)
    {
        gameManager.tileManager.RemoveObject(axis);
    }

    public void CreateObject(Vector2 axis, string type)
    {
        RemoveObject(axis);

        GameObject prefab = GetObjectFromType(type);

        gameManager.tileManager.AddObject(prefab, axis);
    }

    private GameObject GetObjectFromType(string type)
    {
        GameObject result = Resources.Load<GameObject>("Prefabs/" + type);

        if (result != null)
        {
            return result;
        }
        else
        {
            Debug.Log("Wrong prefab name!");
            return null;
        }
    }
    public void ChangeSelectedTile(string targetType)
    {
        Tile targetTile = null;
        if (selectedTile == null)
        {
            return;
        }
        else
        {
            targetTile = selectedTile;
        }

        Vector2 axis = tileManager.GetAxis(targetTile);

        switch (targetType)
        {
            case "None":
                {
                    tileManager.RemoveObject(axis);
                    break;
                }
            case "Block":
                {
                    CreateObject(axis, "Block");
                    break;
                }
            case "FallenStar":
                {
                    CreateObject(axis, "FallenStar");
                    break;
                }
            case "SpaceShip":
                {
                    CreateObject(axis, "SpaceShip");
                    break;
                }
            case "TradePost":
                {
                    CreateObject(axis, "TradePost");
                    break;
                }
        }
    }
}
