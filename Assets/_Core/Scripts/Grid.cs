using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    private int m_width;
    private int m_height;
    private int[,] m_gridArray;

    public Grid(int width, int height)
    {
        m_width = width;
        m_height = height;

        m_gridArray = new int[width, height];

        Debug.Log(m_width + " " + m_height);
    }
}
