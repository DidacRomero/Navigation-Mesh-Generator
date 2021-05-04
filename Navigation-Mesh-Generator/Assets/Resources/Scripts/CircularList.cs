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

    //Linear expression floats
    public float a;
    public float b;

    public Edge(Vector2 vec, Vector2 iPoint) //Constructor
    {
        this.vec = vec;
        this.iPoint = iPoint; //Just for now as we test, later on we will assign the actual value here

        //Fill linear expression data
        a = this.vec.y / this.vec.x;
        b = iPoint.y - a * iPoint.x;
        y = 0;
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
public class BalancedTree : SortedDictionary<Edge, EdgeType>
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

    }

    public void Succ(Edge s, EdgeType t)
    {

    }

    public void Pred(Edge s, EdgeType t)
    {

    }

    public void Find(HMVert p)
    {
        if(p.type == VERTEX_TYPE.START)
        {
            //List<float> keys = new List<float>(this.Keys);  //Get all keys to find the edges above & below
            //foreach(float y in keys)
            //{
                
            //}
            // y = ax + b should be equal than the vertex.y for edges connected to our vertex
            
        }
        else if (p.type == VERTEX_TYPE.END)
        {

        }
        else if (p.type == VERTEX_TYPE.BEND)
        {

        }
    }
};

//public class BalancedTree
//{
//    public TreeNode root;

//    public void Find()
//    {

//    }
//    public void Insert(Edge s, bool in_edge)
//    {
//        //Insert with the y value of the lineal function y = ax + b
//        TreeNode t = new TreeNode();
//        root.children.Add(t);
//    }

//    public void Delete()
//    {
//        if(root != null)
//        {

//        }
//        else
//        {
//            Debug.Log("Tree has no root, therefore it is empty");
//        }
//    }

//    public void Succ()
//    {

//    }

//    public void Prev()
//    {

//    }

//    private TreeNode FindNode(TreeNode t, float y)
//    {
//        TreeNode ret = null;
//        if (root!= null)
//        {
//            if (t.children != null && t.children.Count > 0)
//            {
//                foreach (TreeNode child in t.children)
//                {
//                    if (child.y == y)
//                    {
//                        return child;   //Case where we found the node
//                    }
//                    //Keep searching
//                    ret = FindNode(child, y);
//                }
//            }
//        }
//        return ret;    //
//    }
//};
