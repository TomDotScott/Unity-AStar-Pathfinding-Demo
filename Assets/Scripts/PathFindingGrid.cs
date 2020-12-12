using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class PathFindingGrid<T>
{
    private int m_width;
    private int m_height;
    private float m_cellSize;
    private T[,] m_gridArray;
    private TextMesh[,] m_debugTextArray;
    private Vector3 m_origin;

    public PathFindingGrid(int width, int height, float cellSize, Vector3 origin)
    {
        m_width = width;
        m_height = height;
        m_cellSize = cellSize;
        m_origin = origin;

        m_gridArray = new T[width, height];
        m_debugTextArray = new TextMesh[width, height];

        for (int x = 0; x < m_gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < m_gridArray.GetLength(1); y++)
            {
                m_debugTextArray[x, y] = UtilsClass.CreateWorldText(m_gridArray[x, y].ToString(), null, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * 0.5f, 10, Color.white, TextAnchor.UpperLeft);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
            }
        }
        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * m_cellSize + m_origin;
    }

    private Vector2Int GetXY(Vector3 worldPosition)
    {
        return new Vector2Int(
            Mathf.FloorToInt((worldPosition - m_origin).x / m_cellSize),
            Mathf.FloorToInt((worldPosition - m_origin).y / m_cellSize)
            );
    }

    public void SetValue(int x, int y, T val)
    {
        // Ignore invalid values
        if (x >= 0 && y >= 0 && x < m_width && y < m_height)
        {
            m_gridArray[x, y] = val;
            m_debugTextArray[x, y].text = val.ToString();
        }
    }

    public void SetValue(Vector3 worldPosition, T val)
    {
        Vector2Int xy = GetXY(worldPosition);
        SetValue(xy.x, xy.y, val);
    }

    public T GetValue(int x, int y)
    {
        // Ignore invalid values
        if (x >= 0 && y >= 0 && x < m_width && y < m_height)
        {
            return m_gridArray[x, y];
        }

        return default;
    }

    public T GetValue(Vector3 worldPosition)
    {
        Vector2Int xy = GetXY(worldPosition);
        return GetValue(xy.x, xy.y);
    }
}
