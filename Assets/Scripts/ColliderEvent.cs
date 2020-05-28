using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderEvent : MonoBehaviour
{
    public bool playerInRange;
    public GameObject target;
    public EnemyController self;

    void OnTriggerEnter(Collider obj)
    {
        if (self.chaosModel)
        {
            if (obj.tag == "Player" || obj.tag == "Enemy" || obj.tag == "Blocker")
            {
                playerInRange = true;
                target = obj.gameObject;
            }
        }
        else
        {
            if (obj.tag == "Player")
            {
                playerInRange = true;
                target = obj.gameObject;
            }
        }
    }

    private void OnTriggerExit(Collider obj)
    {
        if (self.chaosModel)
        {
            if (obj.tag == "Player" || obj.tag == "Enemy" || obj.tag == "Blocker")
            {
                playerInRange = false;
                target = null;
            }
        }
        else
        {
            if (obj.tag == "Player")
            {
                playerInRange = false;
                target = null;
            }
        }
    }

}

