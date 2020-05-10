using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderEvent : MonoBehaviour
{
    public bool playerInRange;

    void OnTriggerStay(Collider obj)
    {
        if (obj.tag == "Player")
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider obj)
    {
        if (obj.tag == "Player")
        {
            playerInRange = false;
        }
    }

}

