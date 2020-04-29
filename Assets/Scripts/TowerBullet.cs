using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBullet : MonoBehaviour
{
    private int damage = 0;
    private Player player = null;
    private float moveSpeed = 2f;

    public void SetDamage(int damage)
    {
        this.damage = damage;
    }

    public void SetPlayer(Player player)
    {
        this.player = player;
    }

    public void Fire()
    {
        transform.parent = null;
        gameObject.SetActive(true);
        transform.LookAt(player.transform.position);
        Invoke("DestroyObj", 10.0f);
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * 10 * moveSpeed, Space.Self);
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
