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

[System.Serializable]
public struct Edge
{
    Vector2 e; //Non_Normalized_Edge vector

    public Edge(Vector2 vec)
    {
        e = vec;
    }   //Constructor
};

[System.Serializable]
public class BalancedTree : SortedDictionary<float, Edge>
{

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
