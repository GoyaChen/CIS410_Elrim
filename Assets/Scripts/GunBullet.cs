using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBullet : MonoBehaviour
{
    private int damage = 0;
    private Player player = null;
    private float moveSpeed = 1f;
    private Rigidbody rigid = null;


    public void SetDamage(int damage)
    {
        this.damage = damage;
    }


    public void Fire()
    {
        transform.parent = null;
        gameObject.SetActive(true);
        rigid = GetComponent<Rigidbody>();
        rigid.AddForce(Vector3.forward * Time.deltaTime * moveSpeed);
        Invoke("DestroyObj", 10.0f);
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * 10 * moveSpeed, Space.Self);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Tower")
        {
            other.GetComponent<EnemyTower>().GetDamage(damage);
            gameObject.SetActive(false);
            DestroyObj();
        }
    }

    private void DestroyObj()
    {
        Destroy(gameObject);
    }
}
