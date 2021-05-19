using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WayNode
{
    public List<int> adjacent_nodes;
    public Vector3[] verts;
    public Vector3 pos;
    public Vector3[] bisectors; // array with all bisectors

    public WayNode()   //Default constructor
    {
    }
    public WayNode(Vector3[] vertices)
    {
        verts = vertices;
        pos = (vertices[0] + vertices[1] + vertices[2]) / 3.0f;
    }

    public WayNode(Vector3[] vertices, Vector3 center_pos)
    {
        verts = vertices;
        pos = center_pos;
    }

    //Calculate bisectors
    public void CalculateBisectors()
    {
        if(verts!=null)
        {
            for(int i = 0; i < 3; ++i)
            {
                Vector3 opp_edge = verts[(i + 2) % 3] - verts[(i + 1) % 3];
                Vector3 opp_point = verts[(i + 1) % 3] + opp_edge / 2;
                bisectors[i] = opp_point - verts[i];
            }
        }
    }
};

//Adjacency list to 
public class AdjacencyList : SortedList < int, WayNode >
{

    //Insert a waynode and make sure adjacency is placed in both waynodes
    public void Insert (int tri_id, WayNode n)
    {
        //If this node is not on the list, add the node to the list
        if(!this.ContainsKey(tri_id))
        {
            this.Add(tri_id, n);

            //Check if the adjacent nodes are created
            if(n.adjacent_nodes != null)
            foreach(int adj in n.adjacent_nodes)
            {
                //If they're not, create an empty waypoint
                if (this.ContainsKey(adj) != false)
                    this.Insert(adj, new WayNode());
            }

            //Check all adjacency is respected
            if (n.adjacent_nodes != null)
            foreach (int adj in n.adjacent_nodes)
            {
                //If we are not in our adjacent nod adjacent list, add us there
                if (this[adj].adjacent_nodes.Contains(tri_id) != false)
                {
                    this[adj].adjacent_nodes.Add(tri_id);
                }
            }
        }
    }
};

public class WaypointClasses : MonoBehaviour
{

    //Adjacency List
    AdjacencyList test;
    // Start is called before the first frame update
    void Start()
    {
        test = new AdjacencyList();   
    }

    // Update is called once per frame
    void Update()
    {
        //If the spacebar is pressed
        if (Input.GetKeyDown(KeyCode.Space) == true)
        {
            FillFromMesh(GetComponent<MeshFilter>());
        }
    }

    private void OnDrawGizmos()
    {
        if(test != null && test.Count > 0)
        {
            for(int i = 0; i < test.Count; ++i)
            {
                Gizmos.DrawWireSphere(test[i].pos, 0.14f);
            }
        }
    }

    public void FillFromMesh(MeshFilter mf)
    {
        Mesh m = mf.mesh;

        //Get tris and vertices from mesh
        int[] tris = m.GetTriangles(0);
        List<Vector3> verts = new List<Vector3>();
        m.GetVertices(verts);

        //Iterate triangle per triangle
        int j = 0;
        for (int i = 0; i < tris.Length / 3; ++i)
        {
            j = 3 * i;
            Vector3[] new_verts = new Vector3[3];
            new_verts[0] = verts[tris[j]];
            new_verts[1] = verts[tris[j + 1]];
            new_verts[2] = verts[tris[j + 2]];

            //Insert this WayNode
            WayNode n = new WayNode(new_verts);
            test.Insert(i,n);
        }

        int testing = 0;
    }
}

