using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : MonoBehaviour
{

    [SerializeField] private PathFindingVisual m_pathFindingVisualisation;
    [SerializeField] private float m_speed;
    
    private bool m_canTravelDiagonally = true;

    private PathFinding m_pathFinding;
    private Vector3 m_screenBottomLeft;

    private bool m_startedMoving = false;
    private bool m_endPlaced = false;

    private Vector3 m_endLocation;

    List<PathFindingNode> m_path;

    void Start()
    {
        m_pathFinding = new PathFinding(15, 8);
        m_pathFindingVisualisation.SetGrid(m_pathFinding.Grid);
        m_screenBottomLeft = new Vector3(Camera.main.ScreenToWorldPoint(Vector3.zero).x, Camera.main.ScreenToWorldPoint(Vector3.zero).y, 0);
    }

    private void Update()
    {
        if (!m_startedMoving)
        {
            HandleClicks();
        }
        else
        {
            // As long as there is a path, we have to move the bot
            if (m_path.Count > 1)
            {
                Vector3 newPosition = GridXYToWorld(new Vector2Int(m_path[0].X, m_path[0].Y));

                // Move towards the new position
                transform.position = Vector3.MoveTowards(transform.position, newPosition, m_speed * Time.deltaTime);

                if (Mathf.Abs(Vector3.Distance(transform.position, newPosition)) < 0.1f)
                {
                    // We have reached the new destination so remove the front element
                    m_path.Remove(m_path[0]);
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(m_endLocation.x, m_endLocation.y), m_speed * Time.deltaTime);
            }
        }
    }

    void HandleClicks()
    {
        // Left Click to set the end node
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (m_endPlaced)
            {
                Vector3 diffVector = mousePosition - m_endLocation;
                // If we're on the same cell, clear it
                if (Mathf.Abs(diffVector.x) < 2.5f || Mathf.Abs(diffVector.y) < 2.5f)
                {
                    Vector2Int end = m_pathFinding.Grid.GetXY(mousePosition);
                    PathFindingNode currentNode = m_pathFinding.Grid.GetGridObject(end.x, end.y);
                    currentNode.m_nodeState = PathFindingNode.NodeState.eDefault;
                    m_endPlaced = false;
                }
                else
                {
                    m_endPlaced = true;
                }
            }
            else
            {
                m_endLocation = mousePosition;
                Vector2Int end = m_pathFinding.Grid.GetXY(m_endLocation);
                PathFindingNode currentNode = m_pathFinding.Grid.GetGridObject(end.x, end.y);
                currentNode.m_nodeState = PathFindingNode.NodeState.ePath;
                m_endPlaced = true;
            }

        }
        // Middle click to set the position of the bot
        else if (Input.GetMouseButtonDown(2))
        {
            // Get the vector3 mouse position
            Vector3 vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            gameObject.transform.position = new Vector3(vec.x, vec.y);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            // Get the vector3 mouse position
            Vector3 vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector2Int xy = m_pathFinding.Grid.GetXY(vec);

            PathFindingNode currentNode = m_pathFinding.Grid.GetGridObject(xy.x, xy.y);

            // Set to be unwalkable or walkable if double right clicked
            currentNode.m_isWalkable = !currentNode.m_isWalkable;

            // Change state for visualisation
            currentNode.m_nodeState = currentNode.m_nodeState == PathFindingNode.NodeState.eDefault
                ? PathFindingNode.NodeState.eBlockade
                : PathFindingNode.NodeState.eDefault;
        }
    }



    void GeneratePath(Vector3 start, Vector3 end)
    {
        Vector2Int startLocation = m_pathFinding.Grid.GetXY(start);
        Vector2Int endLocation = m_pathFinding.Grid.GetXY(end);

        m_path = m_pathFinding.FindPath(startLocation, endLocation, m_canTravelDiagonally);
    }

    // Called from a UI Button
    public void StartPathFinding()
    {
        GeneratePath(transform.position, m_endLocation);
        m_startedMoving = true;
    }

    // Called from a UI checkbox
    public void SetCanTravelDiagonally()
    {
        m_canTravelDiagonally = !m_canTravelDiagonally;
    }

    public void SetSpeed(float newSpeed)
    {
        m_speed = newSpeed;
    }

    Vector3 GridXYToWorld(Vector2Int gridXY)
    {
        return ((new Vector3(gridXY.x, gridXY.y) * 5) + m_screenBottomLeft) + (new Vector3(5, 5) * 0.5f);
    }
}
