using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float scale;
    public float damage;
    public bool ismagic;

    private string unitTag;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject.Destroy(gameObject, scale);
    }

    void OnTriggerEnter(Collider other)
    {
        if (gameObject.CompareTag("EnemyBullet"))
        {
            unitTag = "Character";
        }
        else if(gameObject.CompareTag("CharacterBullet"))
        {
            unitTag = "Enemy";
        }
        if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag(unitTag))
        {
            GameObject.Destroy(gameObject,0.0f);
        }
    }
}

   