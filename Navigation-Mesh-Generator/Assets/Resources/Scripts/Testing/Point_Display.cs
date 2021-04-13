using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point_Display : MonoBehaviour
{
    // Start is called before the first frame update

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(this.transform.position, 0.14f);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
