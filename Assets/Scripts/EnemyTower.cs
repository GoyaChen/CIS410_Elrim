using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 敌方塔
/// </summary>
public class EnemyTower : MonoBehaviour
{
    [SerializeField] private Transform Rotator = null;
    [SerializeField] private ColliderEvent attackRangeDetector = null;
    [SerializeField] private GameObject fireEffect = null;//开火效果
    [SerializeField] private ParticleSystem particleEffect = null;//粒子开火效果
    [SerializeField] private AudioSource fireAudio = null;//开火音效
    [SerializeField] private AudioSource continueFireAudio = null;//持续开火音效
    [SerializeField] private ParticleSystem deadEffect = null;//死亡效果
    [SerializeField] private AudioSource deadAudio = null;//死亡音效
    [SerializeField] private Transform bulletPoint = null; //子弹点
    [SerializeField] private TowerBullet bulletPrefab = null; //炮弹
    [SerializeField] private Slider heathUI = null; //血条
    [SerializeField] private Transform heathBar = null; //血条UI
    [Header("基础数据")]
    [SerializeField]
    public int MaxHP;//最大HP
    public int Defense;//防御力
    public float attackSpeed; //攻击速度
    public int attackDamage;//伤害值
    public int explodeDamage = 0;//爆炸伤害
    public float fieldOfFire;//射程
    public float moveSpeed;//子弹速度
    public bool ismagic;//是否是魔法攻击

    private Player player = null;
    private bool playerInRange;
    private int CurHP;
    private float lastAttackTime = 0;
    private bool isDead = false;
    private Vector3 _angles;
    private bool isfireAudioOn;

    void Awake()
    {
        CurHP = MaxHP;
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        heathUI.maxValue = MaxHP;
        isfireAudioOn = false;
    }

    void Update()
    {
        playerInRange = attackRangeDetector.playerInRange;
        if (playerInRange && !isDead && !player.Dead)
        {
            Rotator.LookAt(Rotator.transform.position + Rotator.position - player.transform.position);
            if (Time.time - lastAttackTime >= attackSpeed)
            {
                lastAttackTime = Time.time;
                Attack();
                if (particleEffect != null) particleEffect.Play();
                if (fireAudio != null) fireAudio.Play();
            }
            if (!isfireAudioOn)
            {
                if (continueFireAudio != null) continueFireAudio.Play();
                isfireAudioOn = true;
            }
            if (fireEffect != null) fireEffect.SetActive(true);

        }
        else
        {
            if (fireEffect != null) fireEffect.SetActive(false);
            if (particleEffect != null) particleEffect.Stop();
            if (isfireAudioOn)
            {
                if (continueFireAudio != null) continueFireAudio.Stop();
                isfireAudioOn = false;
            }
           
        }
        heathUI.value = CurHP;
        heathBar.LookAt(heathBar.position + heathBar.position - player.transform.position);

        if (isDead)
        {
            if(deadEffect != null) deadEffect.Play();
            if (deadAudio != null) deadAudio.Play();
            Destroy(gameObject, 2.0f);
        }
    }

    private void Attack()
    {

        TowerBullet newBullet = Instantiate(bulletPrefab, bulletPoint.transform.position, bulletPoint.transform.rotation, bulletPoint.transform.parent);
        newBullet.SetDamage(attackDamage);
        newBullet.SetExplodeDamage(explodeDamage);
        newBullet.SetPlayer(player);
        newBullet.SetmovefieldOfFire(fieldOfFire);
        newBullet.SetmoveSpeed(moveSpeed);
        newBullet.Fire();
    }


    public void GetDamage(int value)
    {
        if (CurHP - value > 0)
        {
            if (ismagic)
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

    public bool GetCanRotate()
    {
        return !playerInRange && !isDead && !player.Dead;
    }
}