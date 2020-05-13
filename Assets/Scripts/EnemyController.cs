using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 敌方塔
/// </summary>
public class EnemyController : MonoBehaviour
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
    [SerializeField] private BulletController bulletPrefab = null; //炮弹
    [SerializeField] private Slider heathUI = null; //血条
    [SerializeField] private GameObject heathBar = null; //血条UI
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
    public float visibleDistance;//可以看到血条的距离

    private PlayerController player = null;
    private bool playerInRange;
    private int CurHP;
    private float lastAttackTime = 0;
    private bool isDead = false;
    private Vector3 _angles;
    private bool isfireAudioOn;
    private bool isDeadAudioOn;
    private float distance;

    void Awake()
    {
        CurHP = MaxHP;
        heathUI.maxValue = MaxHP;
        isfireAudioOn = false;
        isDeadAudioOn = false;
        if (deadEffect != null) deadEffect.Stop();
        heathBar.SetActive(false);
    }

    void Update()
    {
        if (player == null) player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        if (player != null)
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
                Destroy(gameObject, 1.5f);
            }
        }
        
    }

    private void Attack()
    {

        BulletController newBullet = Instantiate(bulletPrefab, bulletPoint.transform.position, bulletPoint.transform.rotation, bulletPoint.transform.parent);
        newBullet.SetDamage(attackDamage);
        newBullet.SetisDamageMagic(ismagic);
        newBullet.SetExplodeDamage(explodeDamage);
        newBullet.SetmovefieldOfFire(fieldOfFire);
        newBullet.SetmoveSpeed(moveSpeed);
        newBullet.tag = this.tag;
        newBullet.Fire();
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

    public bool GetCanRotate()
    {
        return !playerInRange && !isDead && !player.Dead;
    }
}