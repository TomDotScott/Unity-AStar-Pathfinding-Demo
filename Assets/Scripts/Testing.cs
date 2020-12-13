using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    private PathFinding pt;
    private Vector3 bottomLeft;

    [SerializeField] private PathFindingVisual ptVisual;

    void Start()
    {
        pt = new PathFinding(15, 8);
        ptVisual.SetGrid(pt.Grid);
        bottomLeft = new Vector3(Camera.main.ScreenToWorldPoint(Vector3.zero).x, Camera.main.ScreenToWorldPoint(Vector3.zero).y, 0);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Get the vector3 mouse position
            Vector3 vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector2Int xy = pt.Grid.GetXY(vec);

            List<PathFindingNode> path = pt.FindPath(new Vector2Int(0, 0), xy);

            int step = 0;
            foreach (PathFindingNode node in path)
            {
                if (node.m_cameFromNode == null)
                {
                    Debug.Log("STARTING AT: " + node.X + " " + node.Y);
                }
                else
                {
                    Debug.Log("STEP " + step + " GO FROM " + node.m_cameFromNode.X + " " + node.m_cameFromNode.Y + " TO " + node.X + " " + node.Y);
                }
                step++;
            }


            if (path != null)
            {
                for (int i = 0; i < path.Count - 1; i++)
                {
                    Vector3 startPath = new Vector3(path[i].X, path[i].Y) * 10f + Vector3.one * 5f + bottomLeft;
                    Vector3 endPath = new Vector3(path[i + 1].X, path[i + 1].Y) * 10f + Vector3.one * 5f + bottomLeft;
                    Debug.DrawLine(startPath, endPath, Color.green, 5f);
                }
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            // Get the vector3 mouse position
            Vector3 vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector2Int xy = pt.Grid.GetXY(vec);

            PathFindingNode currentNode = pt.Grid.GetGridObject(xy.x, xy.y);

            // Set to be unwalkable or walkable if double right clicked
            currentNode.m_isWalkable = !currentNode.m_isWalkable;

            // Change state for visualisation
            currentNode.m_nodeState = currentNode.m_nodeState == PathFindingNode.NodeState.eDefault
                ? PathFindingNode.NodeState.eBlockade
                : PathFindingNode.NodeState.eDefault;

            Debug.Log(xy.x + " " + xy.y + " Was set to be a blockade");

        }

    }
}
