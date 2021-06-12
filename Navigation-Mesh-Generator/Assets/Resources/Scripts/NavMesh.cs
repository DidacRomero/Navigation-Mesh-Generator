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
    // Start is called before the first frame update
    void Start()
    {
        iv = gameObject.AddComponent<IdentifyVertices>();
        wgraph = gameObject.AddComponent<WaypointGraphGen>();
        test = new AdjacencyList();

        iv.GetMapVertices(); //Obtain the vertices of the map (our composite collider)
        iv.createMesh(); // Creates the mesh from the information of the vertices
        

        GameObject collider = new GameObject();
        collider.gameObject.AddComponent<MeshCollider>();
        MeshCollider mc = collider.GetComponent<MeshCollider>();
        mc.sharedMesh = iv.m;
        collider.layer = 6; //Here goes the NavMesh Layer!

        test.FillFromMesh(GetComponent<MeshFilter>()); //Calculate the adjacency list given our mesh!
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        if (iv != null && iv.m != null)
            MeshGizmosDraw(iv.m);

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
