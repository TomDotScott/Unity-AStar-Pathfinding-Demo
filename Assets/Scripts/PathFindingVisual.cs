using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;

public class PathFindingVisual : MonoBehaviour
{
    private const int FINAL_STATE = 5;

    private PathFindingGrid<PathFindingNode> m_grid;

    private TextMesh[,] m_debugTextArray;

    private Mesh m_mesh;

    private void Awake()
    {
        m_mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = m_mesh;
    }

    public void SetGrid(PathFindingGrid<PathFindingNode> grid)
    {
        m_grid = grid;

        m_debugTextArray = new TextMesh[m_grid.Width, m_grid.Height];

        for (int x = 0; x < m_grid.Width; x++)
        {
            for (int y = 0; y < m_grid.Height; y++)
            {
                m_debugTextArray[x, y] = CodeMonkey.Utils.UtilsClass.CreateWorldText(
                   m_grid.GetGridObject(x, y)?.ToString(),
                   null,
                   m_grid.GetWorldPosition(x, y) + new Vector3(m_grid.CellSize, m_grid.CellSize) * 0.5f,
                   10,
                   Color.white,
                   TextAnchor.MiddleCenter
                   );
            }
        }

        UpdatePathFindingVisual();

        m_grid.OnGridValueChanged += Grid_OnValueChanged;
    }

    private void Update()
    {
        UpdatePathFindingVisual();
    }

    private void Grid_OnValueChanged(object sender, PathFindingGrid<PathFindingNode>.OnGridValueChangedArgs e)
    {
        Debug.Log("Grid_OnValueChanged");
        UpdatePathFindingVisual();
    }


    public void UpdatePathFindingVisual()
    {
        MeshUtils.CreateEmptyMeshArrays(m_grid.Width * m_grid.Height, out Vector3[] vertices, out Vector2[] uvs, out int[] triangles);

        for (int x = 0; x < m_grid.Width; x++)
        {
            for (int y = 0; y < m_grid.Height; y++)
            {
                int index = x * m_grid.Height + y;

                Vector3 quadSize = new Vector3(1, 1) * m_grid.CellSize;

                PathFindingNode node = m_grid.GetGridObject(x, y);

                // Create a normalised value to colour the quad
                float gridValueNormalised = (float)node.m_nodeState / FINAL_STATE;

                Vector2 gridValueUV = new Vector2(gridValueNormalised, 0);

                MeshUtils.AddToMeshArrays(vertices, uvs, triangles, index, m_grid.GetWorldPosition(x, y) + quadSize * 0.5f, 0, quadSize, gridValueUV, gridValueUV);


                m_debugTextArray[x, y].text = node?.ToString();
            }
        }

        m_mesh.vertices = vertices;
        m_mesh.uv = uvs;
        m_mesh.triangles = triangles;
    }
}
