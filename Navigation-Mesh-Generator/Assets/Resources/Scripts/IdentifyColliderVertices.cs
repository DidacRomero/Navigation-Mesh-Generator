using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdentifyColliderVertices : MonoBehaviour
{

    CompositeCollider2D col;
    List<Vector2> verts = new List<Vector2>();
    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<CompositeCollider2D>();

        Bounds b = col.bounds;
        //b.
        

        for (int i = 0; i < col.pathCount; i++)
        {
            Vector2[] pathVerts = new Vector2[col.GetPathPointCount(i)];
            col.GetPath(i, pathVerts);
            verts.AddRange(pathVerts);
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        foreach (Vector2 pos in verts)
        Gizmos.DrawWireSphere(pos, 0.14f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
