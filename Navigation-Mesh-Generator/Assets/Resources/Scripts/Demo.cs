using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Demo : MonoBehaviour
{
    Dropdown meshes_dd = null;
    int d_value = 0;

    GameObject loading_text = null;

    [SerializeField]
    List<GameObject> meshes = null;

    GameObject current = null;

    public bool d_mesh = true;
    public bool d_vertices = true;
    public bool d_adjacency = true;


    //Toggles for visibility of vertices, mesh etc.
    public Toggle draw_mesh = null;
    public Toggle draw_vertices = null;
    public Toggle draw_adjacency = null;
    
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

        loading_text = GameObject.Find("Loading_Text");
        if(loading_text != null)
            loading_text.SetActive(false);

        //Load all meshes
        LoadMeshes();


        //Setup listeners for toggles
        draw_mesh.onValueChanged.AddListener(delegate {
            ToggleValueChanged(draw_mesh);
        });

        draw_vertices.onValueChanged.AddListener(delegate {
            ToggleValueChanged(draw_vertices);
        });

        draw_adjacency.onValueChanged.AddListener(delegate {
            ToggleValueChanged(draw_adjacency);
        });
    }

    // Update is called once per frame
    void Update()
    {
        CheckDrawGizmos();
    }

    private void CheckDrawGizmos()
    {
        NavMesh nm = meshes[d_value].GetComponent<NavMesh>();
        if (nm.generated == true)
        {
            nm.draw_vertices = d_vertices;
            nm.draw_adjacency = d_adjacency;
            nm.draw_mesh = d_mesh;
        }
    }

    void DropdownValueChanged(Dropdown change)
    {
        //Before we change our dropdown value, destroy our previous mesh
        NavMesh nm = meshes[d_value].GetComponent<NavMesh>();
        if (nm.generated == true)
            nm.DestroyNavigationMesh();

        meshes[d_value].SetActive(false);
        d_value = change.value;

        if (d_value < meshes.Count)
            meshes[d_value].SetActive(true);

        Debug.Log("New value: " + change.value);

    }

    void ToggleValueChanged(Toggle change)
    {
        NavMesh nm = meshes[d_value].GetComponent<NavMesh>();
        if (change == draw_vertices)
        {
            d_vertices = change.isOn;
        }
        else if(change == draw_mesh)
        {
            d_mesh = change.isOn;
        }
        else
        {
            d_adjacency = change.isOn;
        }
    }

    public void DrawVerticesChanged(bool value)
    {
        NavMesh nm = meshes[d_value].GetComponent<NavMesh>();
        nm.draw_vertices = value;
    }

    public void CreateNavMesh()
    {
        NavMesh nm = meshes[d_value].GetComponent<NavMesh>();
        nm.CreateNavigationMesh();
    }

    //Load Meshes
    void LoadMeshes()
    {
        if (meshes == null)
            meshes = new List<GameObject>();
        else
            meshes.Clear();

        //Load all prefab meshes
        meshes.Add(Instantiate(Resources.Load<GameObject>("PreFabs/Maps/Extra_Map1")));
        meshes.Add(Instantiate(Resources.Load<GameObject>("PreFabs/Maps/Extra_Map2")));
        meshes.Add(Instantiate(Resources.Load<GameObject>("PreFabs/Maps/Extra_Map3")));
        meshes.Add(Instantiate(Resources.Load<GameObject>("PreFabs/Maps/Extra_Map4")));
        meshes.Add(Instantiate(Resources.Load<GameObject>("PreFabs/Maps/Extra_Map5")));
        meshes.Add(Instantiate(Resources.Load<GameObject>("PreFabs/Maps/Extra_Map6")));
        meshes.Add(Instantiate(Resources.Load<GameObject>("PreFabs/Maps/Grid_Map_Medium")));
        meshes.Add(Instantiate(Resources.Load<GameObject>("PreFabs/Maps/Grid_Map_Small")));
        meshes.Add(Instantiate(Resources.Load<GameObject>("PreFabs/Maps/Grid_Map_Smallest")));

        foreach( GameObject go in meshes)
        {
            if(go != null)
                go.SetActive(false);
        }
        meshes[d_value].SetActive(true);
    }
}
