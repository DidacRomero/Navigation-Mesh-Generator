using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdentifyColliderVertices : MonoBehaviour
{

    CompositeCollider2D col;
    List<Vector2> verts = new List<Vector2>();
    [SerializeField] List<Vector2> pol_verts = new List<Vector2>();


    [SerializeField] private Queue<Vector2> queue  = new Queue<Vector2>();
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

            //saving the outer polygon on a testing list
            if(i ==1 )
            {
                pol_verts.AddRange(pathVerts);
                pol_verts.Reverse();
            }
        }

        //Triangulate the polygon!
        //Triangulate();

        queue = GetXStruct();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        
        //foreach (Vector2 pos in verts)
        //{
        //    Gizmos.DrawWireSphere(pos, 0.14f);
        //}
        for (int i = 0; i < verts.Count; ++i)
        {
            Gizmos.DrawWireSphere(verts[i], 0.14f);

            //Avoid getting out of range on the last vertex
            if(i< pol_verts.Count -1)
            Debug.DrawLine(pol_verts[i], pol_verts[i+1], Color.white,0.01f);
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Sort vertices in ascending X order then ascending y order
    private int SortVert(Vector2 a, Vector2 b)
    {
        //This could be optimized to a simple a -b if we took the time to convert all the vertices info to a worldspace without negative numbers!
        int ax = (int)a.x;
        int bx = (int)b.x;

        if (ax < bx)
            return -1;
        else if (ax > bx)
            return 1;
        else if (ax == bx)
        {
            if (a.y < b.y)
                return -1;
            else if (a.y > b.y)
                return 1;
        }
        return 0;
    }
    //Given a simple polygon, create the X struct (queue sorted by x then y)
    Queue<Vector2> GetXStruct()
    {
        Queue<Vector2> x = new Queue<Vector2>();

        List<Vector2> q = pol_verts;
        q.Sort(SortVert);


        foreach (Vector2 vert in q)
            x.Enqueue(vert);
       
        return x;
    }

    void Triangulate()
    {
        Queue<Vector2> X_st = GetXStruct();
        //x_struct
        //y struct
        //c struct

        //TRI
        //Edges

    }
}
