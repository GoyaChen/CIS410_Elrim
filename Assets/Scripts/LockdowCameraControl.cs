using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockdowCameraControl : MonoBehaviour
{
    public float zoomSpeed;
    public float dragSpeed;

    private Vector2 rootPoint;
    private Vector2 currentPoint;
    private Vector2 moveDir;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            transform.position += new Vector3(0, zoomSpeed, 0);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            transform.position -= new Vector3(0, zoomSpeed, 0);
        }

        if (Input.GetMouseButtonDown(0))
        {
            rootPoint = Input.mousePosition + 3.3333f * new Vector3(transform.position.x, transform.position.z);
        }

        if (Input.GetMouseButton(0))
        {
            currentPoint = Input.mousePosition;
            moveDir = -(currentPoint - rootPoint);
            transform.position = new Vector3(moveDir.x * dragSpeed, transform.position.y, moveDir.y * dragSpeed) ;
        }
    }
}
