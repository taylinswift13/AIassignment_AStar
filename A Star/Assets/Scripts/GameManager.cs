﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EntityType
{
    public int amount;
    public GameObject typePrefab;
}
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public TileManager tileManager;
    public GridManager grid;
    private GameObject initTile;

    public int column;
    public int row;

    public List<EntityType> entities;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
        InitGrid();
        InitMap();
    }

    private void InitMap()
    {
        List<Tile> tilesAvailable = new List<Tile>(grid.tiles);
        GameObject Tiles = new GameObject("Tilemap");
        foreach (EntityType type in entities)
        {
            for (int i = 0; i < type.amount; i++)
            {
                int randomTile = Random.Range(0, tilesAvailable.Count);

                foreach (Tile tile in grid.tiles)
                {
                    if (tilesAvailable[randomTile].position == tile.position)
                    {
                        GameObject entityPrefab = Instantiate(type.typePrefab, tile.position, Quaternion.Euler(0, 0, 0));
                        entityPrefab.transform.SetParent(Tiles.transform);
                        tile.objects.Add(entityPrefab);//add to tiles
                    }
                }
                tilesAvailable.Remove(tilesAvailable[randomTile]);//Prevent from overlapping initialization.
            }
        }
        foreach (Tile tile in grid.tiles)
        {
            GameObject tilePrefab = Instantiate(initTile, tile.position, Quaternion.Euler(0, 0, 0));
            tilePrefab.transform.SetParent(Tiles.transform);
        }
    }

    private void InitGrid()
    {
        Vector2 size = new Vector2(column, row);
        Vector2 position = new Vector2(0, 0);
        grid = new GridManager(size, position);//Initialized the grid
        tileManager = new TileManager(grid.tiles, grid);//Connect the grid and the tiles, instantiate tile at each grid.
        initTile = Resources.Load<GameObject>("Prefabs/Tile");
    }
}
