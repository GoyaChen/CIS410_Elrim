using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagerControler : MonoBehaviour
{
    public ParticleSystem damageEffect = null;

    private int damage;
    private bool isMagic;

    private bool isDamage;

    private void Start()
    {
        isDamage = true;
        if (damageEffect != null) damageEffect.Play();
        Destroy(gameObject, 1.0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (tag == "EnemyBllet")
        {
            if (other.tag == "Player")
            {
                if (isDamage) other.GetComponent<PlayerController>().GetDamage(damage, isMagic);                  
            }
        }
        else if(tag == "PlayerBullet")
        {
            if (other.tag == "Enemy")
            {
                if (isDamage) other.GetComponent<EnemyController>().GetDamage(damage, isMagic);

            }
        }
        isDamage = false;

    }

    public void SetDamage(int damage)
    {
        this.damage = damage;
    }

    public void SetisMagic(bool isMagic)
    {
        this.isMagic = isMagic;
    }
   
}
