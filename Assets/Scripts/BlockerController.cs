using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockerController : MonoBehaviour
{
    [SerializeField] private ParticleSystem deadEffect = null;//死亡效果
    [SerializeField] private AudioSource deadAudio = null;//死亡音效
    [SerializeField] private Slider heathUI = null; //血条
    [SerializeField] private GameObject heathBar = null; //血条UI

    [Header("基础数据")]
    [SerializeField]
    public int MaxHP;//最大HP
    public int Defense;//防御力
    public float visibleDistance;//可以看到血条的距离

    private PlayerController player = null;
    private int CurHP;
    private bool isDead = false;
    private bool isDeadAudioOn;
    private float distance;

    void Awake()
    {
        CurHP = MaxHP;
        heathUI.maxValue = MaxHP;
        isDeadAudioOn = false;
        if (deadEffect != null) deadEffect.Stop();
        heathBar.SetActive(false);
    }

    void Update()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        if(player != null)
        {
            distance = (player.transform.position - transform.position).sqrMagnitude;
            if (distance < visibleDistance)
            {

                heathBar.SetActive(true);
                heathUI.value = CurHP;
                heathBar.transform.LookAt(heathBar.transform.position + heathBar.transform.position - player.transform.position);
            }
            else
            {
                heathBar.SetActive(false);
            }

            if (isDead)
            {
                if (deadEffect != null) deadEffect.Play();
                if (!isDeadAudioOn)
                {
                    if (deadAudio != null) deadAudio.Play();
                    isDeadAudioOn = true;
                }
                Destroy(gameObject);
            }
        }
    }


    public void GetDamage(int value, bool isDamageMagic)
    {
        if (CurHP - value > 0)
        {
            if (isDamageMagic)
            {
                CurHP -= value;
            }
            else
            {
                CurHP -= (value - Defense);
            }

        }
        else
        {
            CurHP = 0;
            isDead = true;
        }
    }
}
