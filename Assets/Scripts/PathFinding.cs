using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding
{
    // define the basic pathfinding costs
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 15;

    private PathFindingGrid<PathFindingNode> m_grid;

    // Nodes to be searched
    private List<PathFindingNode> m_openList;

    // Nodes chosen to traverse
    private List<PathFindingNode> m_closedList;

    public PathFinding(int width, int height)
    {
        Vector3 bottomLeft = new Vector3(Camera.main.ScreenToWorldPoint(Vector3.zero).x, Camera.main.ScreenToWorldPoint(Vector3.zero).y, 0);
        m_grid = new PathFindingGrid<PathFindingNode>(width, height, 5f, bottomLeft, (PathFindingGrid<PathFindingNode> g, int x, int y) => new PathFindingNode(g, x, y));
    }

    private List<PathFindingNode> FindPath(Vector2Int start, Vector2Int end)
    {
        PathFindingNode startNode = m_grid.GetGridObject(start.x, start.y);
        PathFindingNode endNode = m_grid.GetGridObject(end.x, end.y);


        m_openList = new List<PathFindingNode> { startNode };
        m_closedList = new List<PathFindingNode>();

        // Cycle through grid, set g to infinite and calculate f cost
        for (int x = 0; x < m_grid.Width; x++)
        {
            for (int y = 0; y < m_grid.Height; y++)
            {
                // get a reference to the object
                PathFindingNode pathNode = m_grid.GetGridObject(x, y);

                // set g cost to infinite
                pathNode.m_gCost = int.MaxValue;

                // calculate f cost
                pathNode.CalculateFCost();
                pathNode.cameFromNode = null;
            }
        }

        // Calculate the costs of the start node
        startNode.m_gCost = 0;
        startNode.m_hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        // Cycle through all of the nodes and find a path
        while (m_openList.Count > 0)
        {
            // The current node is the node in the openlist with the lowest F-Cost
            PathFindingNode currentNode = GetLowestFCostNode(m_openList);

            // If the currentNode is the end
            if (currentNode == endNode)
            {
                // We have reached the end of the path
                return CalculatePath(endNode);
            }

            m_openList.Remove(currentNode);
            m_closedList.Add(currentNode);

            foreach (var neighbour in GetNeighbourNodes(currentNode))
            {
                // Make sure the current neighbour node isn't in the closed list
                if (!m_closedList.Contains(neighbour))
                {
                    int potentialGCost = currentNode.m_gCost + CalculateDistanceCost(currentNode, neighbour);

                    // see if the potential cost is lower than the cost of the node
                    if (potentialGCost < neighbour.m_gCost)
                    {
                        // Update the node
                        neighbour.cameFromNode = currentNode;
                        neighbour.m_gCost = potentialGCost;

                        // Calculate the new H-Cost for the node
                        neighbour.m_hCost = CalculateDistanceCost(neighbour, endNode);
                        neighbour.CalculateFCost();

                        // if the open list doesn't contain the neighbour, add it
                        if (!m_openList.Contains(neighbour))
                        {
                            m_openList.Add(neighbour);
                        }
                    }
                }
            }
        }

        // If we reach here, there is no path
        return null;
    }

    private List<PathFindingNode> GetNeighbourNodes(PathFindingNode currentNode)
    {
        List<PathFindingNode> neighbourList = new List<PathFindingNode>();

        // Find the 8 neightbour positions if they are valid
        if(currentNode.X - 1 >= 0)
        {
            // Left
            neighbourList.Add(GetNode(currentNode.X - 1, currentNode.Y));
            if(currentNode.Y - 1 >= 0)
            {
                // BL
                neighbourList.Add(GetNode(currentNode.X - 1, currentNode.Y - 1));
            }

            if(currentNode.Y + 1 < m_grid.Height)
            {
                // TL
                neighbourList.Add(GetNode(currentNode.X - 1, currentNode.Y + 1));
            }
        }

        if(currentNode.X + 1 < m_grid.Width)
        {
            // Right
            neighbourList.Add(GetNode(currentNode.X + 1, currentNode.Y));
            if(currentNode.Y - 1 >= 0)
            {
                // BR
                neighbourList.Add(GetNode(currentNode.X + 1, currentNode.Y - 1));
            }

            if(currentNode.Y + 1 < m_grid.Height)
            {
                // TR
                neighbourList.Add(GetNode(currentNode.X + 1, currentNode.Y + 1));
            }
        }

        // Top
        if(currentNode.Y - 1 >= 0)
        {
            neighbourList.Add(GetNode(currentNode.X, currentNode.Y - 1));
        }

        // Bottom
        if(currentNode.Y + 1 < m_grid.Height)
        {
            neighbourList.Add(GetNode(currentNode.X, currentNode.Y + 1));
        }

        return neighbourList;
    }

    private PathFindingNode GetNode(int x, int y)
    {
        return m_grid.GetGridObject(x, y);
    }

    private List<PathFindingNode> CalculatePath(PathFindingNode endNode)
    {
        return null;
    }

    private int CalculateDistanceCost(PathFindingNode a, PathFindingNode b)
    {
        int xDist = Mathf.Abs(a.X - b.X);
        int yDist = Mathf.Abs(a.Y - b.Y);
        int remaining = Mathf.Abs(xDist - yDist);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDist, yDist) + MOVE_STRAIGHT_COST * remaining;
    }

    private PathFindingNode GetLowestFCostNode(List<PathFindingNode> pathFindingNodeList)
    {
        // Initialise to the start
        PathFindingNode lowestFCostNode = pathFindingNodeList[0];

        foreach (PathFindingNode node in pathFindingNodeList)
        {
            // overwrite the lowestNode if the F-Cost is lower
            if (node.m_fCost < lowestFCostNode.m_fCost)
            {
                lowestFCostNode = node;
            }
        }

        return lowestFCostNode;
    }
}