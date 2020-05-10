using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBullet : MonoBehaviour
{
    private int damage;
    private Player player = null;
    private float moveSpeed;
    private float fieldOfFire;
    private float existTime;

    public void SetDamage(int damage)
    {
        this.damage = damage;
    }

    public void SetmoveSpeed(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }

    public void SetmovefieldOfFire(float fieldOfFire)
    {
        this.fieldOfFire = fieldOfFire;
    }

    public void SetPlayer(Player player)
    {
        this.player = player;
    }

    public void Fire()
    {
        existTime = fieldOfFire / moveSpeed;
        transform.parent = null;
        gameObject.SetActive(true);
        transform.LookAt(player.transform.position);
        Invoke("DestroyObj", existTime);
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed, Space.Self);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<Player>().GetDamage(damage);
            gameObject.SetActive(false);
            DestroyObj();
        }
    }

    private void DestroyObj()
    {
        Destroy(gameObject);
    }
}
