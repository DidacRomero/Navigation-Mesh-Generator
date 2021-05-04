using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class y_st_Test : MonoBehaviour
{

    [SerializeField]
    public BalancedTree y_test;
    // Start is called before the first frame update
    void Start()
    {
        y_test = new BalancedTree(new EdgeComparer());

        y_test.Insert(new Edge(new Vector2(2,0), new Vector2(1,1)), EdgeType.IN );
        y_test.Insert(new Edge(new Vector2(2, 0), new Vector2(-1, 0)), EdgeType.IN);
        y_test.Insert(new Edge(new Vector2(0, 1), new Vector2(-1, -1)), EdgeType.OUT);
        y_test.Insert(new Edge(new Vector2(0, 1), new Vector2(1, 0)), EdgeType.OUT);

        //Testing Find
        {
            HMVert vert1 = new HMVert();
            vert1.pos = new Vector2(1,1);
            vert1.type = VERTEX_TYPE.END;

            HMVert vert2 = new HMVert();
            vert2.pos = new Vector2(-1,1);
            vert2.type = VERTEX_TYPE.START;

            HMVert vert3 = new HMVert();
            vert3.pos = new Vector2(-1,0);
            vert3.type = VERTEX_TYPE.START;

            HMVert vert4 = new HMVert();
            vert4.pos = new Vector2(1,0);
            vert4.type = VERTEX_TYPE.END;


            y_test.Find(vert1);
            y_test.Find(vert2);
            y_test.Find(vert3);
            y_test.Find(vert4);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
