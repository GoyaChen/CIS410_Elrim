using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotater : MonoBehaviour
{
    public float rotateSpeed;
    public EnemyTower enemyTower;

    void Update()
    {
        if (enemyTower.GetCanRotate())
        {
            transform.Rotate(new Vector3(0.0f, rotateSpeed, 0.0f));
        }
    }
}
