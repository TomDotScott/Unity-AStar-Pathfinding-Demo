using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathFindingNode
{
    public enum NodeState
    {
        eDefault, eBlockade, eInOpenList, eInClosedList, ePath
    };

    private PathFindingGrid<PathFindingNode> m_grid;
    private int m_x;
    private int m_y;

    public int m_fCost;
    public int m_gCost;
    public int m_hCost;

    public PathFindingNode m_cameFromNode;

    public bool m_isWalkable;

    public NodeState m_nodeState;

    public PathFindingNode(PathFindingGrid<PathFindingNode> grid, int x, int y, bool isWalkable)
    {
        m_grid = grid;
        m_x = x;
        m_y = y;
        m_isWalkable = isWalkable;

        m_nodeState = !m_isWalkable ? NodeState.eBlockade : NodeState.eDefault;
    }

    public int X { get => m_x; }
    public int Y { get => m_y; }

    public void CalculateFCost()
    {
        m_fCost = m_gCost + m_hCost;
    }

    public override string ToString()
    {
        if(m_fCost == int.MaxValue || m_gCost == int.MaxValue || m_fCost < 0 || m_gCost < 0 || m_hCost < 0)
        {
            return "F: " + 0 + "\nG: " + 0 + "\nH: " + 0;
        }
        return "F: " + m_fCost + "\nG: " + m_gCost + "\nH: " + m_hCost;
    }
}