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
    [SerializeField] private GameObject bulletPrefab = null; //炮弹
    [SerializeField] private Slider heathUI = null; //血条
    [SerializeField] private GameObject heathBar = null; //血条UI
    [Header("基础数据")]
    [SerializeField]
    public int MaxHP;//最大HP
    public int Defense;//防御力
    public float attackSpeed; //攻击速度
    public int attackDamage;//伤害值
    public int explodeDamage = 0;//爆炸伤害
    public int conDamage = 0;//持续伤害
    public float fieldOfFire;//射程
    public float moveSpeed;//子弹速度
    public float rotateSpeed;//旋转速度
    public float rotateAngle;//旋转角度
    public bool ismagic;//是否是魔法攻击
    public bool isStraight;//是否是直射攻击
    public bool chaosModel;//混战模式

    private PlayerController player = null;
    private GameObject target = null;
    private bool playerInRange;
    private int CurHP;
    private float lastAttackTime = 0;
    private bool isDead = false;
    private Vector3 _angles;
    private bool isfireAudioOn;
    private bool isDeadAudioOn;
    private float distance;
    private float startAngle;
    private Quaternion originalRotation;
    private bool isOriginal;
    private float timer;
    private float Angle = 0;
    private int rotatedir = 1;

    public bool Dead { get { return isDead; } }

    public bool GetCanRotate()
    {
        return !playerInRange && !isDead;
    }

    void Awake()
    {
        CurHP = MaxHP;
        heathUI.maxValue = MaxHP;
        isfireAudioOn = false;
        isDeadAudioOn = false;
        if (deadEffect != null) deadEffect.Stop();
        heathBar.SetActive(false);
        isOriginal = true;
    }

    void Update()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        if (player != null)
        {
            playerInRange = attackRangeDetector.playerInRange;
            if (playerInRange && !isDead && !player.Dead)
            {
                if (isOriginal)
                {
                    originalRotation = Rotator.transform.rotation;
                    isOriginal = false;
                } 
                target = attackRangeDetector.target;
                if (target != null)
                {
                    if (target != this.gameObject)
                    {
                        Rotator.LookAt(target.transform.position);
                    }
                }
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
                if (!isOriginal)
                {
                    Rotator.transform.rotation = originalRotation;
                    isOriginal = true;
                }
            }
            if(target != null)
            {
                distance = (target.transform.position - transform.position).sqrMagnitude;
            }
            else
            {
                distance = (player.transform.position - transform.position).sqrMagnitude;
            }
            startAngle = -45 * distance / (fieldOfFire * fieldOfFire);
            float visibleDistance = player.fieldOfFire * player.fieldOfFire + 14;
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
                timer += Time.deltaTime;
                if (timer > 1.0f) gameObject.SetActive(false);
            }

            if (GetCanRotate())
            {
                if(Angle >= rotateAngle)
                {
                    rotatedir = -1;
                }
                if (Angle <= -rotateAngle)
                {
                    rotatedir = 1;
                }
                Rotator.transform.Rotate(new Vector3(0.0f, rotatedir * rotateSpeed, 0.0f));
                Angle += (rotateSpeed * rotatedir);
            }
        }
    }

    private void Attack()
    {

        GameObject newBulletPreb = Instantiate(bulletPrefab, bulletPoint.transform.position, bulletPoint.transform.rotation, bulletPoint.transform.parent);
        BulletController newBullet = newBulletPreb.GetComponent<BulletController>();
        newBullet.SetDamage(attackDamage);
        newBullet.SetisDamageMagic(ismagic);
        newBullet.SetExplodeDamage(explodeDamage);
        newBullet.SetConDamage(conDamage);
        newBullet.SetmovefieldOfFire(fieldOfFire);
        newBullet.SetmoveSpeed(moveSpeed);
        newBullet.SetselfCollider(this.GetComponent<Collider>());
        newBullet.tag = "EnemyBullet";
        if (!isStraight) newBullet.transform.Rotate(startAngle, 0, 0, Space.Self);
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
                CurHP -= ((value - Defense) >= 0 ? (value - Defense) : 0);
            }

        }
        else
        {
            CurHP = 0;
            isDead = true;
        }
    }
}