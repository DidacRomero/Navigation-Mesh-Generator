using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct WayNode
{
    public List<int> adjacent_nodes;
};

//Adjacency list to 
public class AdjacencyList : SortedList < int, WayNode >
{

    //Insert a waynode and make sure adjacency is placed in both waynodes
    public void Insert (int tri_id, WayNode n)
    {
        //If this node is not on the list, add the node to the list
        if(this.ContainsKey(tri_id) != false)
        {
            this.Add(tri_id, n);

            //Check if the adjacent nodes are created
            foreach(int adj in n.adjacent_nodes)
            {
                //If they're not, create an empty waypoint
                if (this.ContainsKey(adj) != false)
                    this.Insert(adj, new WayNode());
            }

            //Check all adjacency is respected
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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
