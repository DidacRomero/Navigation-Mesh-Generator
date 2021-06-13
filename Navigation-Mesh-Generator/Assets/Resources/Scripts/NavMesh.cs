using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*-----------------------------------------------------------------------------------
 * This class uses all the required scripts to generate a navigation mesh from start to end.
 * This way you only need to attach this script into a gameobject with the appropriate composite collider
 * to automatically generate the navigation mesh and navigation information.
 */

public class NavMesh : MonoBehaviour
{
    IdentifyVertices iv = null;
    WaypointGraphGen wgraph= null;
    AdjacencyList test = null;
    Mesh mesh = null;
    MeshFilter mf = null;

    LibTessDotNet.Tess Tess = null;
    // Start is called before the first frame update
    void Start()
    {
        iv = gameObject.AddComponent<IdentifyVertices>();
        wgraph = gameObject.AddComponent<WaypointGraphGen>();
        test = new AdjacencyList();

        iv.GetMapVertices(); //Obtain the vertices of the map (our composite collider)
        CreateMesh(); // Creates the mesh from the information of the vertices
        

        GameObject collider = new GameObject();
        collider.gameObject.AddComponent<MeshCollider>();
        MeshCollider mc = collider.GetComponent<MeshCollider>();
        mc.sharedMesh = mesh;
        collider.layer = 6; //Here goes the NavMesh Layer!

        test.FillFromMesh(GetComponent<MeshFilter>()); //Calculate the adjacency list given our mesh!
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private LibTessDotNet.ContourVertex[] CreateContour(List<Vector2> list)
    {
        LibTessDotNet.ContourVertex[] contour = new LibTessDotNet.ContourVertex[list.Count];

        for (int i = 0; i < list.Count; ++i)
        {
            contour[i] = new LibTessDotNet.ContourVertex();
            contour[i].Position = new LibTessDotNet.Vec3(list[i].x, list[i].y, 0.0f);
        }

        return contour;
    }

    //This function transfers the tesselation information onto our Mesh
    private void TessToMesh(LibTessDotNet.Tess T,Mesh m)
    {
        Vector3[] vertices = new Vector3[T.Vertices.Length];

        for (int i = 0; i < T.Vertices.Length; ++i)
        {
            vertices[i] = new Vector3(T.Vertices[i].Position.X, T.Vertices[i].Position.Y, T.Vertices[i].Position.Z);
        }
        m.vertices = vertices;
        m.triangles = T.Elements;

        m.RecalculateNormals();
    }

    public void CreateMesh()
    {
        if(Tess == null)
        Tess = new LibTessDotNet.Tess();

        //Add Outer polygon contour!
        Tess.AddContour(CreateContour(iv.pol_verts), LibTessDotNet.ContourOrientation.CounterClockwise);

        //Adding the contours of the holes
        for (int i = 0; i < iv.holes.Count; ++i)
            Tess.AddContour(CreateContour(iv.holes[i]), LibTessDotNet.ContourOrientation.Clockwise);

        Tess.Tessellate(LibTessDotNet.WindingRule.EvenOdd);

        //Generate a Unity Mesh from the Tesselator information
        if(mesh == null)
        mesh = new Mesh();

        TessToMesh(Tess,mesh);

        //Turn the mesh into a mesh filter
        mf = gameObject.AddComponent<MeshFilter>();
        mf.mesh = mesh;
    }

    private void OnDrawGizmos()
    {
        if (mesh != null)
            MeshGizmosDraw(mesh);

        if (test != null)
            test.GizmosDraw();
    }

    public void MeshGizmosDraw(Mesh mesh)
    {
        //Gizmos.color = Color.gray;
        //Gizmos.DrawMesh(mesh);
        Gizmos.color = Color.yellow;

        List<Vector3> verts = new List<Vector3>();
        if (mesh == null)
            return;

        mesh.GetVertices(verts);

        for (int i = 0; i < verts.Count; ++i)
        {
            Gizmos.DrawWireSphere(verts[i], 0.14f);
        }

        int[] tris = mesh.GetTriangles(0);

        //Avoid getting out of range on the last vertex
        int j = 0;
        for (int i = 0; i < tris.Length / 3; ++i)
        {
            //We need to render the lines a bit over the z position of the mesh
            j = 3 * i;
            Debug.DrawLine(verts[tris[j]], verts[tris[j + 1]], Color.green, 0.01f);
            Debug.DrawLine(verts[tris[j + 1]], verts[tris[j + 2]], Color.green, 0.01f);
            Debug.DrawLine(verts[tris[j + 2]], verts[tris[j]], Color.green, 0.01f);
        }
        //Debug.DrawLine(vertices[1], vertices[2], Color.green, 0.01f);

    }
}