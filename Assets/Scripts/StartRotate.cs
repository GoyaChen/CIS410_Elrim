using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRotate : MonoBehaviour
{
    public float x_angle;
    public float y_angle;
    public float z_angle;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.Rotate(x_angle, y_angle, z_angle, Space.Self);
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(new Vector3(2.0f, 0.0f, 0.0f)); 
    }
}
