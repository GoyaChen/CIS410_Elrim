using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderEvent : MonoBehaviour
{
    public bool playerInRange;
    public GameObject target;
    public EnemyController self;

    private void FixedUpdate()
    {
        if (self.chaosModel)
        {
            playerInRange = true;
        }
    }
    void OnTriggerStay(Collider obj)
    {
        if (obj.tag == "Player")
        {
            playerInRange = true;
        }
        target = obj.gameObject;
    }

    private void OnTriggerExit(Collider obj)
    {
        if (obj.tag == "Player")
        {
            playerInRange = false;
        }
        target = null;
    }

}

