using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeliRotorController : MonoBehaviour
{
    public float rotarSpeed;
    public enum Axis
    {
        X,
        Y,
        Z,
    }
    public Axis RotateAxis;


    private float rotateDegree;
    private Vector3 OriginalRotate;

    void Start()
    {
        OriginalRotate = transform.localEulerAngles;
    }

    void Update()
    {
        rotateDegree += rotarSpeed * Time.deltaTime;
        rotateDegree = rotateDegree % 360;

        switch (RotateAxis)
        {
            case Axis.Y:
                transform.localRotation = Quaternion.Euler(OriginalRotate.x, rotateDegree, OriginalRotate.z);
                break;
            case Axis.Z:
                transform.localRotation = Quaternion.Euler(OriginalRotate.x, OriginalRotate.y, rotateDegree);
                break;
            default:
                transform.localRotation = Quaternion.Euler(rotateDegree, OriginalRotate.y, OriginalRotate.z);
                break;
        }
    }
}
