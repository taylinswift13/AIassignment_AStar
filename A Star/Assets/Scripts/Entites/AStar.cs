using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Node
{
    public Tile tile;
    public int F, G, H;
    public Node parent;

    public Node(Tile tile)
    {
        this.tile = tile;
    }
}
public class AStar
{
    private static int MOVECOST = 10;    //orthogonal

    private List<Node> openList;
    private List<Node> closedList;

    private GridManager gird;
    private TileManager tileManager;

    public AStar(GridManager gird, TileManager tileManager)
    {
        this.gird = gird;
        this.tileManager = tileManager;

        openList = new List<Node>();
        closedList = new List<Node>();
    }

    public List<Tile> GetPath(Tile start, Tile end)
    {
        Node startNode = new Node(start);
        Node endNode = new Node(end);
        openList = new List<Node>();
        closedList = new List<Node>();
        openList.Add(startNode);

        while (openList.Count != 0)
        {
            Node current = GetLeastFNode();
            openList.Remove(current);
            closedList.Add(current);

            List<Node> adjacentNodes = GetAdjacentNodes(current);

            foreach (Node node in adjacentNodes)
            {
                if (IfExsited(openList, node) == null)
                {
                    node.parent = current;

                    node.G = Gcost(current, node);
                    node.H = Hcost(node, endNode);
                    node.F = Fcost(node);

                    openList.Add(node);
                }
                else
                {
                    int newGcost = Gcost(current, node);
                    if (newGcost < node.G)
                    {
                        node.parent = current;
                        node.G = newGcost;
                        node.F = Fcost(node);
                    }
                }

                Node temp = IfExsited(openList, endNode);    //Check if reached end node
                if (temp != null)
                {
                    List<Tile> result = new List<Tile>();

                    while (temp.parent != null)
                    {
                        result.Add(temp.tile);
                        temp = temp.parent;
                    }
                    result.Reverse();                        //reverse the order of the list
                    return result;
                }
            }
        }

        return null;
    }

    private Node IfExsited(List<Node> nodes, Node target)
    {
        foreach (Node node in nodes)
        {
            if (node.tile == target.tile)
            {
                return node;
            }
        }
        return null;
    }

    private Node GetLeastFNode()
    {
        if (openList.Count != 0)
        {
            Node result = openList[0];

            foreach (Node node in openList)
            {
                if (node.F < result.F)
                {
                    result = node;
                }
            }
            return result;
        }
        return null;
    }

    private List<Node> GetAdjacentNodes(Node center)
    {
        List<Node> result = new List<Node>();

        List<Tile> tempCloseList = new List<Tile>();

        foreach (Node node in closedList)
        {
            tempCloseList.Add(node.tile);
        }

        List<Tile> tiles = tileManager.GetAdjecentTiles(tileManager.GetAxis(center.tile));

        for (int i = tiles.Count - 1; i >= 0; i--)
        {
            if (tileManager.IfTypeInTile(tiles[i], "Block"))    //If it contains wall then remove the tile
            {
                tiles.Remove(tiles[i]);
            }
        }

        foreach (Tile tile in tiles)
        {
            if (tempCloseList.Contains(tile) == false)
            {
                result.Add(new Node(tile));
            }
        }
        return result;
    }

    private int Gcost(Node parent, Node target)
    {
        int exraG = MOVECOST;
        int parentG = 0;
        if (parent != null)
        {
            parentG = parent.G;
        }
        return exraG + parentG;
    }

    private int Hcost(Node target, Node end)
    {
        Vector2 targetAxis = tileManager.GetAxis(target.tile);
        Vector2 endAxis = tileManager.GetAxis(end.tile);

        return (int)(Mathf.Abs((targetAxis.x - endAxis.x)) + Mathf.Abs((targetAxis.y - endAxis.y))) * MOVECOST;
    }

    private int Fcost(Node node)
    {
        return node.G + node.H;
    }
}
