using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VERTEX_TYPE
{
    START = 0,
    END,
    BEND
}

[System.Serializable]
public struct HMVert
{
    public Vector2 pos;
    public Vector2 edge1;
    public Vector2 edge2;
    public bool proper;
    public float angle;
    public VERTEX_TYPE type;
};

public class HMIdentifyVertices : MonoBehaviour
{

    CompositeCollider2D col;
    List<Vector2> verts = new List<Vector2>();
    [SerializeField] List<Vector2> pol_verts = new List<Vector2>();
    [SerializeField] List<HMVert> hm_verts = new List<HMVert>();

    //Test Mesh
    Mesh m = null;
    MeshFilter mf = null;


    [SerializeField] private Queue<HMVert> queue  = new Queue<HMVert>();
    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<CompositeCollider2D>();

        Bounds b = col.bounds;
        //b.
        

        for (int i = 0; i < col.pathCount; i++)
        {
            Vector2[] pathVerts = new Vector2[col.GetPathPointCount(i)];
            col.GetPath(i, pathVerts);
            verts.AddRange(pathVerts);

            //saving the outer polygon on a testing list
            if(i == 1 )
            {
                pol_verts.AddRange(pathVerts);
                pol_verts.Reverse();        //Now the polygon is counterclock-wise

                //Passing the list to a HM vertex list that will be our queue later
                for(int j = 0; j < pol_verts.Count; ++j)
                {
                    HMVert hm_v = new HMVert();
                    hm_v.pos = pol_verts[j];

                    //Fill edges and angle information for each vertex
                    //Check cases for 1st and last vertices
                    if (j == 0)
                    {
                        //1st vertex
                        hm_v.edge1 = hm_v.pos - pol_verts[pol_verts.Count - 1];
                        hm_v.edge2 = pol_verts[j + 1] - hm_v.pos;
                    }
                    else if(j == pol_verts.Count - 1)
                    {
                        //last vertex
                        hm_v.edge1 = hm_v.pos - pol_verts[j - 1];
                        hm_v.edge2 = pol_verts[0] - hm_v.pos;
                    }
                    else   //Most usual case
                    {
                        hm_v.edge1 = hm_v.pos - pol_verts[j - 1];
                        hm_v.edge2 =  pol_verts[j + 1] - hm_v.pos;
                    }
                    hm_v.angle = Vector2.Angle(hm_v.edge1,hm_v.edge2);

                    //Determine vertex type
                    if ((int)(hm_v.edge1.x) < 0)
                    {
                        if ((int)(hm_v.edge2.x) < 0)
                            hm_v.type = VERTEX_TYPE.BEND;
                        else
                        {
                            hm_v.type = VERTEX_TYPE.START;
                        }
                    }
                    else if((int)(hm_v.edge1.x) > 0)
                    {
                        if ((int)(hm_v.edge2.x) > 0)
                            hm_v.type = VERTEX_TYPE.BEND;
                        else
                        {
                            hm_v.type = VERTEX_TYPE.END;
                        }
                    }
                    else if ((int)(hm_v.edge1.x) == 0)  //SPECIAL CASE !!! but quite common for tilemaps
                    {
                        if ((int)(hm_v.edge2.x) > 0)
                            hm_v.type = VERTEX_TYPE.START;
                        else if ((int)(hm_v.edge2.x) <= 0)
                            hm_v.type = VERTEX_TYPE.END;

                    }
                        

                    //Add the HM_vertex to the list
                    hm_verts.Add(hm_v);
                }
            }
        }

        //Triangulate the polygon!
        //Triangulate();

        queue = GetXStruct();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        
        for (int i = 0; i < verts.Count; ++i)
        {
            Gizmos.DrawWireSphere(verts[i], 0.14f);

            //Avoid getting out of range on the last vertex
            if(i< pol_verts.Count -1)
            Debug.DrawLine(pol_verts[i], pol_verts[i+1], Color.white,0.01f);
        }

        if (m == null)
            return;

        Gizmos.DrawMesh(m);
    }

    // Update is called once per frame
    void Update()
    {
       //var test = LibTess
       
        if(Input.GetKeyDown(KeyCode.Space) == true)
        {
            createMesh();
        }
    }

    public void createMesh()
    {
        var Tess = new LibTessDotNet.Tess();

        LibTessDotNet.ContourVertex[] contour = new LibTessDotNet.ContourVertex[pol_verts.Count];

        for(int i = 0; i < pol_verts.Count; ++i)
        {
            contour[i] = new LibTessDotNet.ContourVertex();
            contour[i].Position = new LibTessDotNet.Vec3(pol_verts[i].x, pol_verts[i].y, 0.0f);
        }

        //Add a contour
        Tess.AddContour( contour, LibTessDotNet.ContourOrientation.CounterClockwise);

        Tess.Tessellate();

        //Test to generate a mesh
        m = new Mesh();

        Vector3[] vertices = new Vector3[pol_verts.Count];

        //for (int i = 0; i < pol_verts.Count; ++i)
        //{
        //    vertices[i] = new Vector3(pol_verts[i].x, pol_verts[i].y, 0.0f);
        //}

        for(int i = 0; i < Tess.Vertices.Length; ++i)
        {
            vertices[i] = new Vector3(Tess.Vertices[i].Position.X, Tess.Vertices[i].Position.Y, Tess.Vertices[i].Position.Z) ;
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

    //Sort vertices in ascending X order then ascending y order
    private int SortVert(HMVert a, HMVert b)
    {
        //This could be optimized to a simple a -b if we took the time to convert all the vertices info to a worldspace without negative numbers!
        int ax = (int)a.pos.x;
        int bx = (int)b.pos.x;

        if (ax < bx)
            return -1;
        else if (ax > bx)
            return 1;
        else if (ax == bx)
        {
            if (a.pos.y < b.pos.y)
                return -1;
            else if (a.pos.y > b.pos.y)
                return 1;
        }
        return 0;
    }
    //Given a simple polygon, create the X struct (queue sorted by x then y)
    Queue<HMVert> GetXStruct()
    {
        Queue<HMVert> x = new Queue<HMVert>();

        List<HMVert> q = hm_verts;
        q.Sort(SortVert);


        foreach (HMVert vert in q)
            x.Enqueue(vert);
       
        return x;
    }

    void Triangulate()
    {
        Queue<HMVert> X_st = GetXStruct();
        //x_struct
        //y struct
        CStruct c_st = new CStruct();

        //TRI
        //Edges
    }
}
