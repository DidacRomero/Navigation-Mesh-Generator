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
        this.CalculateBisectors();
        this.adjacent_nodes = new List<int>();
    }

    public WayNode(Vector3[] vertices, Vector3 center_pos)
    {
        verts = vertices;
        pos = center_pos;
        this.CalculateBisectors();
        this.adjacent_nodes = new List<int>();
    }

    //Calculate bisectors
    public void CalculateBisectors()
    {
        if(verts!=null)
        {
            bisectors = new Vector3[3];
            for(int i = 0; i < 3; ++i)
            {
                Vector3 opp_edge = verts[(i + 2) % 3] - verts[(i + 1) % 3];
                Vector3 opp_point = verts[(i + 1) % 3] + opp_edge / 2;
                bisectors[i] = opp_point - verts[i];
            }
            return;
        }
        Debug.LogWarning("Couldn't calculate bisectors for the WayNode with pos: " + this.pos +" (verts are null)");
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

    //This function populates the adjacency lists of each wayNode, 
    //this function is therefore intended to be used with lists which have no adjacency information yet
    public void CalculateAdjacency()
    {
        if(this.Count == 0)
        {
            Debug.LogWarning("Can't calculate Adjacency on an empty list!");
            return;
        }
        //Setting the layerMask to raycast only against the navigation mesh, 
        //remember to match this number to whichever layer in your project represents the navigation meshes!
        int layerMask = 1 << 6; 

        //Iterate the list
        for (int i = 0; i < this.Count; ++i)
        {
            if (this[i].adjacent_nodes == null)
                this[i].adjacent_nodes = new List<int>();
            //Since each triangle may have up to 3 adjacent nodes, we won't need to do anything on nodes which already have 3 adjacent nodes
            if (this[i].adjacent_nodes.Count < 3)
            {
                //we find adjacent nodes with the bisectors and a raycast
                for( int j = 0; j < this[i].bisectors.Length; ++j)
                {
                    Vector3 point = this[i].verts[j] + this[i].bisectors[j] + this[i].bisectors[j] * 0.01f;
                    //point += this[i].bisectors[j] * 0.1f;
                    Vector3 point2 = point;
                    point2.z -= 0.1f;
                    //Vector2 point = new Vector2(this[i].verts[j].x  +  this[i].bisectors[j].x, this[i].verts[j].y + this[i].bisectors[j].y);
                    RaycastHit hit;
                    Ray ray = new Ray(point2,point - point2);
                    Debug.DrawRay(ray.origin,ray.direction * 1.0f,Color.red, 100.0f);
                    if(Physics.Raycast(ray, out hit, 1.0f, layerMask))
                    {
                        //Use the index of the triangle hit to fill the adjacency information
                        if(!this[i].adjacent_nodes.Contains(hit.triangleIndex))
                        this[i].adjacent_nodes.Add(hit.triangleIndex);

                        //Adjacency goes both ways so add our node to the adjacent node adjacent nodes list
                        if(this[hit.triangleIndex].adjacent_nodes != null && !this[hit.triangleIndex].adjacent_nodes.Contains(i))
                        {
                            this[hit.triangleIndex].adjacent_nodes.Add(i);
                        }
                    }
                }
            }
        }
    }

    //This function takes the information of a mesh filter and calls al the necessary functions to insert all waynodes
    //and calculates the adjacency using CalculateAdjacency()
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
            Insert(i, n);
        }
        CalculateAdjacency();
    }

    public void GizmosDraw()
    {
        Gizmos.color = Color.white;
        if (this != null && this.Count > 0)
        {
            for (int i = 0; i < this.Count; ++i)
            {
                Gizmos.DrawWireSphere(this[i].pos, 0.14f);
                if (this[i].adjacent_nodes != null)
                {
                    for (int j = 0; j < this[i].adjacent_nodes.Count; ++j)
                        Debug.DrawLine(this[i].pos, this[this[i].adjacent_nodes[j]].pos, Color.magenta, 0.01f);
                }
            }
        }
    }

    public void AdjacentNodesDraw(int id)
    {
        Gizmos.color = Color.yellow;
        if (this != null && this.Count > 0)
        {
            if (this[id].adjacent_nodes != null)
            {
                for (int i = 0; i < this[id].adjacent_nodes.Count; ++i)
                    Debug.DrawLine(this[id].pos, this[this[id].adjacent_nodes[i]].pos, Color.yellow, 5.0f);
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
            float time = Time.realtimeSinceStartup;
            test.FillFromMesh(GetComponent<MeshFilter>());
            float fTime = Time.realtimeSinceStartup - time;
            Debug.Log("Process took: " + fTime + " Seconds" );
        }

        //Testing a raycast
       if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit, 100.0f))
            {
                Debug.Log("The raycast hit!");
            }
        }
    }

    private void OnDrawGizmos()
    {
        GizmosDraw();
    }

    public void GizmosDraw()
    {
        if (test != null && test.Count > 0)
        {
            for (int i = 0; i < test.Count; ++i)
            {
                Gizmos.DrawWireSphere(test[i].pos, 0.14f);
                if (test[i].adjacent_nodes != null)
                {
                    for (int j = 0; j < test[i].adjacent_nodes.Count; ++j)
                        Debug.DrawLine(test[i].pos, test[test[i].adjacent_nodes[j]].pos, Color.magenta, 0.01f);
                }
            }
        }
    }
}

