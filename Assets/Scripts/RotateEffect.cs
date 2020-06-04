using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateEffect : MonoBehaviour
{
    public float rotateSpeed;
    public bool x;
    public bool y;
    public bool z;

    // Update is called once per frame
    void Update()
    {
        if (x)
        {
            transform.Rotate(new Vector3(rotateSpeed, 0, 0));
        }

        if (y)
        {
            transform.Rotate(new Vector3(0, rotateSpeed, 0));
        }

        if (z)
        {
            transform.Rotate(new Vector3(0, 0, rotateSpeed));
        }
    }
}
