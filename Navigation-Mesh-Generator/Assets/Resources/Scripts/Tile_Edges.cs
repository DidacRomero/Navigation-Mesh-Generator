using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_Edges : MonoBehaviour
{
    float offset_x = 0.5f;
    float offset_y = 0.5f;


    [HideInInspector] // Hides var below
    Vector3[] vertices = new Vector3[4];

    // Start is called before the first frame update
    void Start()
    {
        //Determine the vertices positions
        vertices[0] = new Vector3(transform.position.x - offset_x, transform.position.y + offset_y, transform.position.z);
        vertices[1] = new Vector3(transform.position.x - offset_x, transform.position.y - offset_y, transform.position.z);
        vertices[2] = new Vector3(transform.position.x + offset_x, transform.position.y - offset_y, transform.position.z);
        vertices[3] = new Vector3(transform.position.x + offset_x, transform.position.y + offset_y, transform.position.z);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        if(vertices[0] != null)
        {
            for(int i = 0; i < 4; ++i)
            {
                Gizmos.DrawWireSphere(vertices[i],0.14f);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
