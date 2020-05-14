using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHitControler : MonoBehaviour
{
    public ParticleSystem hitEffect = null;
    public int ExplodeDamage;//爆炸伤害
    private bool isDamage;

    private void Start()
    {
        isDamage = true;
        if (hitEffect != null) hitEffect.Play();
        Destroy(gameObject, 1.0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (isDamage)
            {
                other.GetComponent<PlayerBase>().GetDamage(ExplodeDamage);
                isDamage = false;
            }
            
            
        }
    }

   
}
