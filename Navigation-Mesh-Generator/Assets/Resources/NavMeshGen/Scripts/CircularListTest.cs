using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularListTest : MonoBehaviour
{
    [SerializeField] CircularList<Vector2> test = new CircularList<Vector2>();
    // Start is called before the first frame update
    void Start()
    {
        test.Add(new Vector2(0.0f, 0.0f));
        test.Add(new Vector2(1.0f, 0.0f));
        test.Add(new Vector2(2.0f, 0.0f));
        test.Add(new Vector2(3.0f, 0.0f));

    }

    //Quick function to test circularity of the list
    void Test()
    {
        for(int i = 0; i < test.Count; ++i)
        {
            Vector2 a = test[i];
            Vector2 next = test.GetNext(i);
            Vector2 previous = test.GetPrevious(i);
            Debug.Log(i + " Vector: " + a + "   Previous" + previous + "    Next:" + next);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Test();
        }
    }
}
