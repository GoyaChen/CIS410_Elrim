using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public GameObject explodeDamager = null;//着弹
    private Collider selfCollider;
    private int damage;
    private int explodDamage;
    private int conDamage;
    private float moveSpeed;
    private float fieldOfFire;
    private float existTime;
    private bool isDamage;
    private bool isDamageMagic;
    private GameObject player = null;
    public float rotate_speed = 0;

    public void SetDamage(int damage)
    {
        this.damage = damage;
    }

    public void SetConDamage(int conDamage)
    {
        this.conDamage = conDamage;
    }

    public void SetisDamageMagic(bool isDamageMagic)
    {
        this.isDamageMagic = isDamageMagic;
    }

    public void SetExplodeDamage(int explodeDamage)
    {
        this.explodDamage = explodeDamage;
    }

    public void SetmoveSpeed(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }

    public void SetmovefieldOfFire(float fieldOfFire)
    {
        this.fieldOfFire = fieldOfFire;
    }

    public void SetselfCollider(Collider selfCollider)
    {
        this.selfCollider = selfCollider;
    }

    public void Fire()
    {
        existTime = fieldOfFire / moveSpeed;
        transform.parent = null;
        gameObject.SetActive(true);
        if (tag == "Enermy")
        {
            player = GameObject.FindWithTag("Player");
            if (player != null) transform.LookAt(player.transform.position);
        }
        Invoke("DestroyObj", existTime);
    }

    private void Start()
    {
        isDamage = true;
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed, Space.Self);
        gameObject.transform.Rotate(new Vector3(rotate_speed, 0.0f, 0.0f));
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other != selfCollider)
        {
            if (other.tag == "Player")
            {
                if (isDamage)
                {
                    other.GetComponent<PlayerController>().GetDamage(damage, isDamageMagic);
                    isDamage = false;
                }
                if (explodeDamager != null) Explode();
                DestroyObj();
            }
            if (other.tag == "Enemy")
            {
                if (isDamage)
                {
                    other.GetComponent<EnemyController>().GetDamage(damage, isDamageMagic);
                    isDamage = false;
                }
                if (explodeDamager != null) Explode();
                DestroyObj();
            }
            else if (other.tag == "Blocker")
            {
                if (isDamage)
                {
                    other.GetComponent<BlockerController>().GetDamage(damage, isDamageMagic);
                    isDamage = false;
                }
                if (explodeDamager != null) Explode();
                DestroyObj();
            }
        }

        if (!isDamageMagic)
        {
            if (other.tag == "Wall" || other.tag == "Ground")
            {
                if (explodeDamager != null) Explode();
                DestroyObj();
            }
        }
    }

    private void Explode()
    {
        GameObject newBullethit = Instantiate(explodeDamager, transform.position, transform.rotation);
        newBullethit.SetActive(true);
        newBullethit.tag = this.tag;
        DamagerControler newDamageController = newBullethit.GetComponent<DamagerControler>();
        newDamageController.SetDamage(explodDamage);
        newDamageController.SetisMagic(isDamageMagic);
        newDamageController.SetConDamage(conDamage);
        newDamageController.SetselfCollider(selfCollider);
    }

    private void DestroyObj()
    {
        Destroy(gameObject);
    }
}
