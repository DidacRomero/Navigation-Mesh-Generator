using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    public Mesh mesh;

    Vector3[] vertices;
    int[] triangles;
    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        //create the mesh
        CreateTestMesh2();

        mesh.Clear(); //Clear prevoius information

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();

        MeshFilter mfil = GetComponent<MeshFilter>();
        mfil.mesh = mesh;
        this.gameObject.AddComponent<MeshCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawMesh(mesh);
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
        for(int i = 0; i < tris.Length / 3; ++i)
        {
            //We need to render the lines a bit over the z position of the mesh
            j = 3 * i;
            Debug.DrawLine(verts[tris[j]], verts[tris[j + 1]], Color.green, 0.01f);
            Debug.DrawLine(verts[tris[j+1]], verts[tris[j + 2]], Color.green, 0.01f);
            Debug.DrawLine(verts[tris[j + 2]], verts[tris[j]], Color.green, 0.01f);
        }
        //Debug.DrawLine(vertices[1], vertices[2], Color.green, 0.01f);

    }

    void CreateTestMesh()
    {
        vertices = new Vector3[]
        {
            new Vector3(0,2,0),                //1
            new Vector3(0,1.2f,0),          //2
            new Vector3(1,1.8f,0),          //3
            new Vector3(2,1,0),             //4
            new Vector3(2.2f,2,0),      //5
            new Vector3(2.8f,1.8f,0),      //6
            new Vector3(3,1,0),             //7
            new Vector3(2,0,0),             //8
            new Vector3(3,0,0),      //9
            new Vector3(4,1.2f,0),      //10
            new Vector3(5,0.8f,0),      //11
            new Vector3(6,1.2f,0)     //12
         };

        triangles = new int[]
        {
            //0, 1, 2,
            //1, 3, 2,
            //3, 4, 2,
            //3, 5, 4,
            //3, 6, 5,
            //3, 7, 8,
            //3, 8, 6,
            //8, 9, 6,
            //8, 10, 11,
            //10, 11, 9



            0, 2, 1,
            1, 2, 3,
            3, 2, 4,
            3, 4, 5,
            3, 5, 6,
            3, 6, 8,
            3, 8, 7,
            8, 6, 9,
            8, 9, 10,
            9, 11, 10
        };
    }


    void CreateTestMesh2()
    {
        vertices = new Vector3[]
       {
            new Vector3(5,25,0),                //1
            new Vector3(7,27,0),          //2
            new Vector3(5,21,0),          //3
            new Vector3(8,24,0),             //4
            new Vector3(9,30,0),      //5
            new Vector3(10,30,0),      //6
            new Vector3(13,32,0),             //7
            new Vector3(15,33,0),             //8
            new Vector3(21,33,0),      //9
            new Vector3(18,30,0),      //10
            new Vector3(15,29,0),      //11
            new Vector3(12,26,0),     //12
            new Vector3(10,22,0),      //13
            new Vector3(13,23,0),      //14
            new Vector3(15,26,0),      //15
            new Vector3(16,23,0),      //16
            new Vector3(20,27,0),      //17
            new Vector3(23,29,0),      //18
            new Vector3(24,27,0),      //19
            new Vector3(27,22,0),      //20
            new Vector3(23,19,0),      //21
            new Vector3(20,20,0),      //22
            new Vector3(17,20,0),      //23
            new Vector3(29,20,0),      //24
            new Vector3(25,15,0),      //25
            new Vector3(27,12,0),      //26
            new Vector3(22,10,0),      //27
            new Vector3(21,5,0),      //28
            new Vector3(21,13,0),      //29
            new Vector3(17,11,0),      //30
            new Vector3(14,8,0),      //31
            new Vector3(9,14,0),      //32
            new Vector3(14,14,0),      //33
            new Vector3(10,18,0)      //34
       };

        triangles = new int[]
       {
            0, 1, 3, //1
            1,4,3, //2
            2, 3, 12, //3
            2,0,3, //4
            4,5,3, //5
            3,5,11, //6
            20,23,24, //7
            5,6,10, //8
            5, 10,11, //9
            21,20,28, //10
            24,25,26, //11
            31,29,30, //12
            31,33,32, //13
            11,14,15, //14
            7, 8,9, //15
            16,18,19, //16
            9,17,16, // 17
            10,9,16, // 18
            10,16,14, //19
            2, 12, 33, //20
            15,16,21, //21
            29,26,27, //22
            26,25,27, //23
            16, 19, 20, //24
            24,23,25, // 25
            28,20, 24, // 26
            20,19,23, // 27
            2,33,31, // 28
            30, 29, 27, //29
            29, 28, 26, //30
            9,8,17, // 31
            10, 7, 9, //32
            11, 10, 14, //33
            6,7,10, //34
            14, 16, 15, //35
            3, 11, 13, //36
            12, 13, 33, // 37
            3, 13, 12, // 38
            31,32,29, // 39
            28,24,26, // 40
            16, 20, 21, //41
            15,21,22, // 42
            11,15,13, // 43
            16,17,18 //44
       };

    }
}
