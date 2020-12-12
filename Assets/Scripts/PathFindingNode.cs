using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingNode
{
    private PathFindingGrid<PathFindingNode> m_grid;
    private int m_x;
    private int m_y;

    public int m_fCost;
    public int m_gCost;
    public int m_hCost;

    public PathFindingNode cameFromNode;

    public int X { get => m_x; }
    public int Y { get => m_y; }

    public PathFindingNode(PathFindingGrid<PathFindingNode> grid, int x, int y)
    {
        m_grid = grid;
        m_x = x;
        m_y = y;
    }

    public void CalculateFCost()
    {
        m_fCost = m_gCost + m_hCost;
    }

    public override string ToString()
    {
        return "X: " + m_x + " Y: " + m_y;
    }
}