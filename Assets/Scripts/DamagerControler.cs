using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagerControler : MonoBehaviour
{
    public ParticleSystem damageEffect = null;
    public float disappearTime;

    private int damage;
    private int conDamage;
    private bool isMagic;

    private bool isDamaged;

    private void Start()
    {
        isDamaged = false;
        if (damageEffect != null) damageEffect.Play();
        Destroy(gameObject, disappearTime);
    }

    private void OnTriggerStay(Collider other)
    {
        print(damage);
        print(isMagic);
        if (tag == "EnemyBullet")
        {
            if (other.tag == "Player")
            {
                other.GetComponent<PlayerController>().GetDamage(conDamage, isMagic);
                if (!isDamaged) other.GetComponent<PlayerController>().GetDamage(damage, isMagic);
                isDamaged = true;
            }
        }
        else if(tag == "PlayerBullet")
        {
            if (other.tag == "Enemy")
            {
                other.GetComponent<EnemyController>().GetDamage(conDamage, isMagic);
                if (!isDamaged) other.GetComponent<EnemyController>().GetDamage(damage, isMagic);
                isDamaged = true;
            }else if (other.tag == "Blocker")
            {
                other.GetComponent<BlockerController>().GetDamage(conDamage, isMagic);
                if (!isDamaged) other.GetComponent<BlockerController>().GetDamage(damage, isMagic);
                isDamaged = true;
            }
            
        }
    }

    public void SetDamage(int damage)
    {
        this.damage = damage;
    }

    public void SetisMagic(bool isMagic)
    {
        this.isMagic = isMagic;
    }

    public void SetConDamage(int conDamage)
    {
        this.conDamage = conDamage;
    }
   
}
