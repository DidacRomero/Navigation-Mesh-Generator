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
        y_test = new BalancedTree();

        y_test.Add(1,new Edge(new Vector2(0.5f,3)) );
        y_test.Add(6, new Edge(new Vector2(4.5f, -3)) );
        y_test.Add(-4, new Edge(new Vector2(-1.5f, 27)) ); 
        y_test.Add(10, new Edge(new Vector2(5, 5.27f)) );
        y_test.Add(0, new Edge(new Vector2(-37, 0)) );
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
