using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    private Grid m_grid;

    // Start is called before the first frame update
    void Start()
    {
        m_grid = new Grid(4, 2, 3f);
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
