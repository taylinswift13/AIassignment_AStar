using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarChaser : Entity
{
    public float moveFrequency = 1f;
    public bool carryingStar;
    public int maxStamina = 50;
    public int stamina = 0;
    SpriteRenderer renderer;
    public GameObject starPrefab;
    public List<LineDrawer> lineDrawers = new List<LineDrawer>();
    public int collectedStars = 0;
    protected override void Init()
    {
        stamina = maxStamina;
        Type = "StarChaser";
        renderer = gameObject.GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        SwitchState(new StarChaserState_Find());
    }
    private void Update()
    {
        base.Update();
        if (carryingStar)
        {
            renderer.color = new Color(1, 1, 0, 1);
        }
        else
        {
            renderer.color = new Color(1, 1, 1, 1);
        }
    }
    public void LineCleaner()
    {
        foreach (LineDrawer lineDrawer in lineDrawers)
        {
            lineDrawer.Destroy();
        }
        lineDrawers.Clear();
    }

    public void PrintPath(List<Tile> path, int nextPos)
    {
        if (path != null && path.Count != 0 && nextPos < path.Count)
        {
            for (int i = nextPos; i < path.Count; i++)
            {
                if (i + 1 < path.Count)
                {
                    LineDrawer lineDrawer = new LineDrawer(0.2f);
                    lineDrawer.DrawLine(path[i].position, path[i + 1].position, Color.white);
                    lineDrawers.Add(lineDrawer);
                }
            }
            LineDrawer line = new LineDrawer(0.2f);
            line.DrawLine(transform.position, path[0].position, Color.white);
            lineDrawers.Add(line);
        }
    }
}



public struct LineDrawer
{
    private LineRenderer lineRenderer;
    private float lineSize;

    public LineDrawer(float lineSize = 0.2f)
    {
        GameObject lineObj = new GameObject("Line");
        lineRenderer = lineObj.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Hidden/Internal-Colored"));
        this.lineSize = lineSize;
    }
    //Draws lines through the provided vertices
    public void DrawLine(Vector3 start, Vector3 end, Color color)
    {
        //Set color
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;

        //Set width
        lineRenderer.startWidth = lineSize;
        lineRenderer.endWidth = lineSize;

        //Set line count which is 2
        lineRenderer.positionCount = 2;

        //Set the postion of both two lines
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }

    public void Destroy()
    {
        if (lineRenderer != null)
        {
            UnityEngine.Object.Destroy(lineRenderer.gameObject);
        }
    }
}
