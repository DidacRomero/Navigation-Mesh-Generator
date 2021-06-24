using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Movement : MonoBehaviour
{
    public float mov_speed = 7.0f;
    public float speed_multiplier = 2.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = new Vector3(0.0f, 0.0f, 0.0f);
        
        if (Input.anyKey)
        {
            float n_speed = mov_speed * Time.deltaTime;

            if (Input.GetKey(KeyCode.LeftShift))
                n_speed *= speed_multiplier;

            //Vertical Input
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                movement.y += n_speed;
            }
            
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                movement.y += -n_speed;
            }

            //Horizontal Input
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                movement.x += n_speed;
            }
            
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                movement.x += -n_speed;
            }
        }

        transform.position += movement;
    }
}
