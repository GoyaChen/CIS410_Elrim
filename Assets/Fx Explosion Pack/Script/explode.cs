using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explode : MonoBehaviour
{
    public GameObject explosion;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z)){
            GameObject new_explose = Instantiate(explosion, transform.position, transform.rotation, transform.parent);
            new_explose.SetActive(true);
;        }
    }
}
