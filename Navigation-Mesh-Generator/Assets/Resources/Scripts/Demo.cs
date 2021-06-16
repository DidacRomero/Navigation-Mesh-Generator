using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Demo : MonoBehaviour
{
    Dropdown meshes_dd = null;
    int d_value = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        meshes_dd = GameObject.Find("Meshes_Dropdown").GetComponent<Dropdown>();

        List<string> m_DropOptions = new List<string> { "Mesh 1", "Mesh 2", "Mesh 3", "Mesh 4", "Mesh 5", "Mesh 6", "Mesh 7", "Mesh 8", "Mesh 9", "Mesh 10" };

        meshes_dd.ClearOptions();
        meshes_dd.AddOptions(m_DropOptions);

        meshes_dd.onValueChanged.AddListener(delegate {
            DropdownValueChanged(meshes_dd);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DropdownValueChanged(Dropdown change)
    {
        d_value = change.value;
        Debug.Log("New value: " + change.value);
    }
}
