using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointGraphGen : MonoBehaviour
{
    public Mesh mesh; //We can asssign meshes here instead of getting our own mesh if we want to

    Vector3[] tris_center;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    //Try to get the mesh, return true if we have a proper mesh, return false if mesh is still null
    bool GetMesh()
    {
        if (mesh == null)
        {
            //If the mesh was not assigned get our own mesh
            mesh = GetComponent<MeshFilter>().mesh;
        }
        return mesh != null;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return) == true)
        {
            GetMesh();
            GenerateFromMesh(mesh);
        }
    }

    private void OnDrawGizmos()
    {
        if(tris_center != null)
        {
            for (int i = 0; i < tris_center.Length; ++i)
            {
                Gizmos.DrawWireSphere(tris_center[i], 0.14f);
            }
        }
    }

    void GenerateFromMesh(Mesh m)
    {
        //Make sure we are working with valid data
        if(mesh == null)
        {
            Debug.LogWarning("Mesh received in GenerateFromMesh was null, can't create a Waypoint Graph");
            return;
        }

        tris_center = GetTrianglesCenter(m);
    }

    //This function assumes the mesh given isn't null
    Vector3[] GetTrianglesCenter(Mesh m)
    {
        int[] tris = m.GetTriangles(0);
        List<Vector3> verts = new List<Vector3>();
        m.GetVertices(verts);

        Vector3[] ret = new Vector3[tris.Length/3]; 
        int j = 0;
        for (int i = 0; i < tris.Length / 3; ++i)
        {
            j = 3 * i;
            Vector3 p = (verts[tris[j]] + verts[tris[j + 1]] + verts[tris[j + 2]]) / 3;
            ret[i] = p;
        }
        return ret;
    }
}
