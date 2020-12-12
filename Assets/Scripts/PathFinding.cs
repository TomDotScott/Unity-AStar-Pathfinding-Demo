using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding
{
    PathFindingGrid<PathFindingNode> m_grid;

    public PathFinding(int width, int height)
    {
       m_grid = new PathFindingGrid<PathFindingNode>(width, height, 10f, Vector3.zero, (PathFindingGrid<PathFindingNode> g, int x, int y) => new PathFindingNode(g, x, y));
    }
}