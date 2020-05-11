using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBullet : MonoBehaviour
{
    public GameObject BulletHit = null;//着弹
    private int damage;
    private int ExplodDamage;
    private Player player = null;
    private float moveSpeed;
    private float fieldOfFire;
    private float existTime;
    private bool isDamage;

    public void SetDamage(int damage)
    {
        this.damage = damage;
    }

    public void SetExplodeDamage(int explodeDamage)
    {
        this.ExplodDamage = explodeDamage;
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

    private void Start()
    {
        isDamage = true;
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed, Space.Self);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (isDamage)
            {
                other.GetComponent<Player>().GetDamage(damage);
                isDamage = false;
            }
            Explode();
            DestroyObj();
        }
        else if (other.tag == "Wall" || other.tag == "Ground")
        {
            Explode();
            DestroyObj();
        }
    }

    private void Explode()
    {
        GameObject newBullethit = Instantiate(BulletHit, transform.position, transform.rotation);
        newBullethit.SetActive(true);
        BulletHitControler newBHC = newBullethit.GetComponent<BulletHitControler>();
        newBHC.ExplodeDamage = ExplodDamage;
    }

    private void DestroyObj()
    {
        Destroy(gameObject);
    }
}
