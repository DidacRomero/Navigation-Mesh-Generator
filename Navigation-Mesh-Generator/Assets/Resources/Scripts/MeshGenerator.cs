using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;
    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        //create the mesh
        CreateTestMesh();

        mesh.Clear(); //Clear prevoius information

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }

    // Update is called once per frame
    void Update()
    {
        
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
            3,2,4,
            3,4,5,
            3,5,6,
            3,6,8,
            3,8,7,
            8,6,9,
            8,9,10,
            9,11,10
        };
    }
}
