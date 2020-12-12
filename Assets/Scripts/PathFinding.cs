using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding
{
    PathFindingGrid<PathFindingNode> m_grid;

    public PathFinding(int width, int height)
    {
        Vector3 bottomLeft = new Vector3(Camera.main.ScreenToWorldPoint(Vector3.zero).x, Camera.main.ScreenToWorldPoint(Vector3.zero).y, 0);
       m_grid = new PathFindingGrid<PathFindingNode>(width, height, 5f, bottomLeft, (PathFindingGrid<PathFindingNode> g, int x, int y) => new PathFindingNode(g, x, y));
    }
}