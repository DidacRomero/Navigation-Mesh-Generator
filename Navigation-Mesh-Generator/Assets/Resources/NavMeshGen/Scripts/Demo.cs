using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class Demo : MonoBehaviour
{
    Dropdown meshes_dd = null;
    int d_value = 0;

    GameObject loading_text = null;

    [SerializeField]
    List<GameObject> meshes = null;

    public bool d_mesh = true;
    public bool d_vertices = true;
    public bool d_adjacency = true;


    //Toggles for visibility of vertices, mesh etc.
    public Toggle draw_mesh = null;
    public Toggle draw_vertices = null;
    public Toggle draw_adjacency = null;

    //Vars to test number of nodes using original 2D approach
    Tilemap tilemap = null;
    Sprite s = null;
    GameObject[] tiles = null;

    //vars for timing
    public int iterations = 10;
    InputField input_iterations = null;
    
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

        input_iterations = GameObject.Find("Input_Iterations").GetComponent<InputField>();

        //Load all meshes
        LoadMeshes();


        //Setup listeners for toggles & UI Input field
        draw_mesh.onValueChanged.AddListener(delegate {
            ToggleValueChanged(draw_mesh);
        });

        draw_vertices.onValueChanged.AddListener(delegate {
            ToggleValueChanged(draw_vertices);
        });

        draw_adjacency.onValueChanged.AddListener(delegate {
            ToggleValueChanged(draw_adjacency);
        });

        input_iterations.onValueChanged.AddListener(delegate {
            ChangeIterations(input_iterations);
        });
    }

    // Update is called once per frame
    void Update()
    {
        CheckDrawGizmos();

        //Check for a click on the mesh
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                Debug.Log("The raycast hit triangle with index: " + hit.triangleIndex);
                NavMesh nm = meshes[d_value].GetComponent<NavMesh>();
                nm.DrawAdjacentNodes(hit.triangleIndex);
            }
        }
    }

    private void OnDrawGizmos()
    {
        
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

        tiles = null;

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

    public float CreateNavMesh()
    {
        NavMesh nm = meshes[d_value].GetComponent<NavMesh>();
        //Time before
        float time = Time.realtimeSinceStartup;
        nm.CreateNavigationMesh();
        time = Time.realtimeSinceStartup - time;

        //Count total nodes of the navigation mesh
        Text t = GameObject.Find("Node_Count").GetComponent<Text>();
        t.text = "Total Nodes: " + nm.AdjacencyListCount();

        //Count all nodes of the traditional pathfinding
        //if(s == null)
        //    s = Resources.Load("Tilesets/tileset_dungeon.png") as Sprite;

        Text t2 = GameObject.Find("2DNode_Count").GetComponent<Text>();
        t2.text = "Original 2D Nodes: " + GetTileAmmounts();

        return time;
    }

    public void CreateNavMeshNoTimer()
    {
        NavMesh nm = meshes[d_value].GetComponent<NavMesh>();
        //Time before
        nm.CreateNavigationMesh();

        //Count total nodes of the navigation mesh
        Text t = GameObject.Find("Node_Count").GetComponent<Text>();
        t.text = "Total Nodes: " + nm.AdjacencyListCount();

        //Count all nodes of the traditional pathfinding
        if (s == null)
            s = Resources.Load("Tilesets/tileset_dungeon.png") as Sprite;

        Text t2 = GameObject.Find("2DNode_Count").GetComponent<Text>();
        t2.text = "Original 2D Nodes: " + GetTileAmmounts();
    }

    //Load Meshes
    void LoadMeshes()
    {
        if (meshes == null)
            meshes = new List<GameObject>();
        else
            meshes.Clear();

        //Load all prefab meshes
        meshes.Add(Instantiate(Resources.Load<GameObject>("NavMeshGen/PreFabs/Maps/Extra_Map1")));
        meshes.Add(Instantiate(Resources.Load<GameObject>("NavMeshGen/PreFabs/Maps/Extra_Map2")));
        meshes.Add(Instantiate(Resources.Load<GameObject>("NavMeshGen/PreFabs/Maps/Extra_Map3")));
        meshes.Add(Instantiate(Resources.Load<GameObject>("NavMeshGen/PreFabs/Maps/Extra_Map4")));
        meshes.Add(Instantiate(Resources.Load<GameObject>("NavMeshGen/PreFabs/Maps/Extra_Map5")));
        meshes.Add(Instantiate(Resources.Load<GameObject>("NavMeshGen/PreFabs/Maps/Extra_Map6")));
        meshes.Add(Instantiate(Resources.Load<GameObject>("NavMeshGen/PreFabs/Maps/Grid_Map_Medium")));
        meshes.Add(Instantiate(Resources.Load<GameObject>("NavMeshGen/PreFabs/Maps/Grid_Map_Small")));
        meshes.Add(Instantiate(Resources.Load<GameObject>("NavMeshGen/PreFabs/Maps/Grid_Map_Smallest")));
        meshes.Add(Instantiate(Resources.Load<GameObject>("NavMeshGen/PreFabs/Maps/Grid_Map_Giant")));


        foreach ( GameObject go in meshes)
        {
            if(go != null)
                go.SetActive(false);
        }
        meshes[d_value].SetActive(true);
    }

    //This function is explictly copied from this forum post https://answers.unity.com/questions/1674467/count-the-amount-of-a-certain-tile-in-a-tilemap.html
    public int GetTileAmmounts()
    {
        if (tiles == null)
        {
            tiles = GameObject.FindGameObjectsWithTag("Walkable");
            if (tiles == null)
                return -1;
        }

        return tiles.Length;
    }

    //This function repeats the process of generating a navigation mesh over and over and returns the average time in milliseconds
    public float CalcGenerationSpeed(int iterations)
    {
        float total_time = 0.0f;
        for(int i = 0; i < iterations; ++i)
        {
            total_time +=  CreateNavMesh();
            //Destroy so we can repeat the process from scratch
            NavMesh nm = meshes[d_value].GetComponent<NavMesh>();
            if (nm.generated == true)
                nm.DestroyNavigationMesh();
        }

        return total_time/(float)iterations;
    }

    public void ChangeIterations(InputField change)
    {
        if(change.text != "")
        iterations = int.Parse(change.text);
    }

    public void IterateAndTime()
    {
        NavMesh nm = meshes[d_value].GetComponent<NavMesh>();
        if (nm.generated == true)
            nm.DestroyNavigationMesh();

        float time = CalcGenerationSpeed(iterations);
        Text t = GameObject.Find("Timer").GetComponent<Text>();
        t.text = "Average navmesh generation time: " + (time*1000.0f) + " ms";
    }
}
