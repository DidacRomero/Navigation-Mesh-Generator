using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdentifyVertices : MonoBehaviour
{
    CompositeCollider2D col;
    List<Vector2> verts = new List<Vector2>();
    [SerializeField] public List<Vector2> pol_verts = new List<Vector2>();
    [SerializeField] public List<List<Vector2>> holes = new List<List<Vector2>>();

    //Test Mesh
    public Mesh m = null;
    MeshFilter mf = null;

    // Start is called before the first frame update
    void Start()
    {
        //GetMapVertices();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        for (int i = 0; i < verts.Count; ++i)
        {
            Gizmos.DrawWireSphere(verts[i], 0.14f);

            //Avoid getting out of range on the last vertex
            if (i < pol_verts.Count - 1)
                Debug.DrawLine(pol_verts[i], pol_verts[i + 1], Color.white, 0.01f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //var test = LibTess

        //if (Input.GetKeyDown(KeyCode.Space) == true)
        //{
        //    createMesh();
        //}
    }

    //This function gets all the necesary references to fill the data structures with the information about vertices
    public void GetMapVertices()
    {
        col = GetComponent<CompositeCollider2D>();

        Bounds b = col.bounds;

        for (int i = 0; i < col.pathCount; i++)
        {
            Vector2[] pathVerts = new Vector2[col.GetPathPointCount(i)];
            col.GetPath(i, pathVerts);
            verts.AddRange(pathVerts);

            //saving the outer polygon on a list
            if (i == 1)
            {
                pol_verts.AddRange(pathVerts);
                pol_verts.Reverse();        //Now the polygon is counterclock-wise  IMPORTANT for Winding Rules! http://www.glprogramming.com/red/chapter11.html 
            }
            else  if ( i > 1)//Case for nested holes
            {
                List<Vector2> l = new List<Vector2>();
                l.AddRange(pathVerts);
                l.Reverse();            //Now the polygon HOLE is clock-wise  IMPORTANT for Winding Rules!

                //Maybe I have to do: if (i == pair number) reverse the list so the holes list only contains clockwise polygons
                holes.Add(l); //Add the list of vertices of the current hole to the holes list
            }
        }
    }

    public void createMesh()
    {
        var Tess = new LibTessDotNet.Tess();

        LibTessDotNet.ContourVertex[] contour = new LibTessDotNet.ContourVertex[pol_verts.Count];

        for (int i = 0; i < pol_verts.Count; ++i)
        {
            contour[i] = new LibTessDotNet.ContourVertex();
            contour[i].Position = new LibTessDotNet.Vec3(pol_verts[i].x, pol_verts[i].y, 0.0f);
        }

        //Add a contour
        Tess.AddContour(contour, LibTessDotNet.ContourOrientation.CounterClockwise);

        //Testing the addition of holes
        for (int i = 0; i < holes.Count; ++i)
        {
            LibTessDotNet.ContourVertex[] hole_contour = new LibTessDotNet.ContourVertex[pol_verts.Count];
            
            for (int j = 0; j < holes[i].Count; ++j)
            {
                hole_contour[j] = new LibTessDotNet.ContourVertex();
                hole_contour[j].Position = new LibTessDotNet.Vec3(holes[i][j].x, holes[i][j].y, 0.0f);
            }
            Tess.AddContour(hole_contour, LibTessDotNet.ContourOrientation.Clockwise);
            //Tess.Tessellate(LibTessDotNet.WindingRule.EvenOdd);
        }

        Tess.Tessellate(LibTessDotNet.WindingRule.EvenOdd);



        //Test to generate a mesh
        m = new Mesh();

        //Count all vertices
        int total_verts = 0;
        total_verts += pol_verts.Count;
        foreach (List<Vector2> list in holes)
        {
            total_verts += list.Count;
        }
        Vector3[] vertices = new Vector3[Tess.Vertices.Length];

        //for (int i = 0; i < pol_verts.Count; ++i)
        //{
        //    vertices[i] = new Vector3(pol_verts[i].x, pol_verts[i].y, 0.0f);
        //}

        for (int i = 0; i < Tess.Vertices.Length; ++i)
        {
            vertices[i] = new Vector3(Tess.Vertices[i].Position.X, Tess.Vertices[i].Position.Y, Tess.Vertices[i].Position.Z);
        }
        m.vertices = vertices;
        m.triangles = Tess.Elements;

        m.RecalculateNormals();

        //Turn the mesh into a mesh filter
        AddMeshFilter();
    }

    public void AddMeshFilter()
    {
        mf = gameObject.AddComponent<MeshFilter>();
        mf.mesh = m;
    }
}