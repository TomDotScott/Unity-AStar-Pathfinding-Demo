﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using System;

public class PathFindingGrid<TGridObject>
{
    private int m_width;
    private int m_height;

    public event EventHandler<OnGridValueChangedArgs> OnGridValueChanged;
    public class OnGridValueChangedArgs : EventArgs
    {
        public int m_x;
        public int m_y;
    }

    private float m_cellSize;
    private TGridObject[,] m_gridArray;
    private Vector3 m_origin;

    public int Width { get => m_width; }
    public int Height { get => m_height; }
    public float CellSize { get => m_cellSize; }

    public PathFindingGrid(int width, int height, float cellSize, Vector3 origin, Func<PathFindingGrid<TGridObject>, int, int, TGridObject> createGridObject)
    {
        m_width = width;
        m_height = height;
        m_cellSize = cellSize;
        m_origin = origin;

        m_gridArray = new TGridObject[width, height];

        // Initilise grid with objects
        for (int x = 0; x < m_gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < m_gridArray.GetLength(1); y++)
            {
                m_gridArray[x, y] = createGridObject(this, x, y);
            }
        }


        for (int x = 0; x < m_gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < m_gridArray.GetLength(1); y++)
            {
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

    public Vector2Int GetXY(Vector3 worldPosition)
    {
        return new Vector2Int(
            Mathf.FloorToInt((worldPosition - m_origin).x / m_cellSize),
            Mathf.FloorToInt((worldPosition - m_origin).y / m_cellSize)
            );
    }

    public void SetGridObject(int x, int y, TGridObject val)
    {
        // Ignore invalid values
        if (x >= 0 && y >= 0 && x < m_width && y < m_height)
        {
            m_gridArray[x, y] = val;
            OnGridValueChanged?.Invoke(this, new OnGridValueChangedArgs { m_x = x, m_y = y });
        }
    }

    public void SetGridObject(Vector3 worldPosition, TGridObject val)
    {
        Vector2Int xy = GetXY(worldPosition);
        SetGridObject(xy.x, xy.y, val);
    }

    public void TriggerGridObjectChanged(int x, int y)
    {

    }

    public TGridObject GetGridObject(int x, int y)
    {
        // Ignore invalid values
        if (x >= 0 && y >= 0 && x < m_width && y < m_height)
        {
            return m_gridArray[x, y];
        }

        return default;
    }

    public TGridObject GetGridObject(Vector3 worldPosition)
    {
        Vector2Int xy = GetXY(worldPosition);
        return GetGridObject(xy.x, xy.y);
    }
}
