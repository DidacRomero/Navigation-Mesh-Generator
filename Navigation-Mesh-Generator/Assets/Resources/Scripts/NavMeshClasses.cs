using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularList<T> : List<T>//This class extends the functionality of a list to a Circular List
{
    public T GetNext(int i)
    {
        T ret;
        if(i == this.Count - 1)
            ret = this[0];
        else
            ret = this[i+1];

        return ret;
    }

    public T GetPrevious(int i)
    {
        T ret;
        if (i == 0)
            ret = this[this.Count - 1];
        else
            ret = this[i - 1];

        return ret;
    }

};

public class CStruct
{
    CircularList<HMVert> L; //List of vertices that form the polygonal chain of vertices that arent' completed yet
    HMVert RM;  //Rightmost vertex

    public HMVert GetRM()
    {
        HMVert ret;

        ret = L[0];
        foreach(HMVert vert in L)
        {
            if (vert.pos.x > ret.pos.x)
                ret = vert;
        }

        RM = ret;   //Placing this line here avois code replication!
        return ret;
    }
};


public class TreeNode
{
    public List<TreeNode> children;
    public TreeNode parent;
    public float y;
};


public enum EdgeType
{
    IN =1,
    OUT = 0
}

[System.Serializable]
public class Edge
{
    public float y; //Variable used for sorting, y value according to y = ax +b at the moment of insertion
    Vector2 vec; //Non_Normalized_Edge vector
    Vector2 iPoint; //initial point

    //This bool will be used when comparing edges, our comparer should never return 0 when the sorted list is sorting edges, 
    //but we need to return 0 when searcing a given edge or else we will never find it 
    public bool search_edge;

    //Linear expression floats
    public float a;
    public float b;

    public Edge(Vector2 vec, Vector2 iPoint) //Constructor
    {
        this.vec = vec;
        this.iPoint = iPoint; //Just for now as we test, later on we will assign the actual value here

        //Fill linear expression data
        if (vec.x != 0) //We could have the special case of a completely vertical edge
            a = this.vec.y / this.vec.x;
        else
            a = 1000;
        b = iPoint.y - a * iPoint.x;
        y = 0;
        search_edge = false;
    }
};

public class EdgeComparer : IComparer<Edge>
{
    public int Compare(Edge i, Edge j)
    {
        if (i.y > j.y)
            return 1;
        else if (i.y < j.y)
            return -1;
        else //if (i.y == j.y)
        {
            if (i.search_edge == true || j.search_edge == true) //If the comparison is being made to find a specific edge in the List
                return 0;
            //Here we avoid returning a 0 since the sorted dictionary would reject the edge saying a key is already there,
            //But the reality is that at a same given point we can have the same y value if both edges intersect
            //We choose to compare the by higher b value
            if (i.b > j.b)
                return 1;
            else
            return -1;
        }
    }
}

[System.Serializable]
public class BalancedTree : SortedList<Edge, EdgeType>
{
    float x;

    public BalancedTree(IComparer<Edge> comp) : base(comp)  //SortedDIctionary Constructor with IComparer
    {
    }
    //Insert with sorting value determined by y = ax + b
    public void Insert(Edge s, EdgeType t)
    {
        s.y = s.a * x + s.b;
        this.Add(s,t);
    }

    public void Delete(Edge s, EdgeType t)
    {
        this.Remove(s);
    }

    public Edge Succ(Edge s, EdgeType t)
    {
        s.search_edge = true;
        if (this.ContainsKey(s))
        {
            int pos = this.IndexOfKey(s);

            List<Edge> k = new List<Edge>();
            k.AddRange(this.Keys);

            //return the upper edge
            if(pos < k.Count-1)
            return k[pos + 1];
        }
        Debug.Log("Key with Y:" + s.y + " was not found on sorted dictionary");

        return null;
    }

    public Edge Pred(Edge s, EdgeType t)
    {
        s.search_edge = true;
        if (this.ContainsKey(s))
        {
            int pos = this.IndexOfKey(s);

            List<Edge> k = new List<Edge>();
            k.AddRange(this.Keys);

            //return the upper edge
            if(pos > 0)
            return k[pos - 1];
        }
        Debug.Log("Key with Y:" + s.y + " was not found on sorted dictionary");

        return null;
    }

    public List<Edge> Find(HMVert p)
    {
        List<Edge> ret = new List<Edge>();
        
        int edge1 = 0;
        int edge2 = 0;
        bool edge1_found = false;
        bool edge2_found = false;
        List<Edge> k = new List<Edge>();
        k.AddRange(this.Keys);

        float y = 0;
        for(int i = 0; i < k.Count; ++i)
        {
            //Calculate y value & compare
            y = k[i].a * x + k[i].b;

            if(y == p.pos.y)    //If the point is contained in the edge
            {
                if (edge1_found == false)
                {
                    edge1 = i;
                    edge1_found = true;
                }
                else
                {
                    edge2 = i;
                    edge2_found = true;
                }
            }
        }

        if (p.type == VERTEX_TYPE.START || p.type == VERTEX_TYPE.END)
        {
            if(edge1_found)
            ret.Add(k[edge1]);

            if(edge2_found)
            ret.Add(k[edge2]);
        }
        else if (p.type == VERTEX_TYPE.BEND)
        {
            if (edge1_found)
                ret.Add(k[edge1]);
        }

        return ret;
    }
};