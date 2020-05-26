using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRotate : MonoBehaviour
{
    public float x_angle;
    public float y_angle;
    public float z_angle;
    public float rotate_speed;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.Rotate(x_angle, y_angle, z_angle, Space.Self);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        gameObject.transform.Rotate(new Vector3(rotate_speed, 0.0f, 0.0f)); 
    }
}
