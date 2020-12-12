using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    private PathFindingGrid<int> m_grid;

    // Start is called before the first frame update
    void Start()
    {
        m_grid = new PathFindingGrid<int>(4, 2, 3f, new Vector3(-35, 15, 0));
    }

    // Update is called once per frame
    void Update()
    {
        // Set values on a mouse click
        if (Input.GetMouseButtonDown(0))
        {
            // Get the vector3 mouse position
            Vector3 vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // if it's a 0, set it to 1, if it's a 1, set it to 0
            m_grid.SetValue(vec, m_grid.GetValue(vec) == 0 ? 1 : 0);
        }
    }
}
