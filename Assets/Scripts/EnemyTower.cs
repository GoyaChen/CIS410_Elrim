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
    [SerializeField] private Transform rotater = null;
    [SerializeField] private ColliderEvent attackRangeDetector = null;
    [SerializeField] private Transform bulletPoint = null; //子弹点
    [SerializeField] private TowerBullet bulletPrefab = null; //炮弹
    [SerializeField] private Slider heathUI = null; //血条
    [SerializeField] private Transform heathBar = null; //血条UI

    private Player player = null;
    private bool playerInRange = false;
    [Header("最大血量")]
    [SerializeField]
    private int MaxHP = 100;
    [Header("当前血量")]
    [SerializeField]
    private int CurHP = 100;
    private float attackSpeed = 70; //攻击速度
    private int attackDamage = 10;//伤害值
    private float lastAttackTime = 0;
    private bool isDead = false;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        attackRangeDetector.OnColliderTriggerEnter += OnColliderTriggerEnter;
        attackRangeDetector.OnColliderTriggerExit += OnColliderTriggerExit;
        heathUI.maxValue = MaxHP;
    }


    private void Update()
    {
        if (playerInRange && !isDead && !player.Dead)
        {
            if (Time.time - lastAttackTime >= 100 / attackSpeed)
            {
                lastAttackTime = Time.time;
                Attack();
            }
        }
        heathUI.value = CurHP;
        heathBar.LookAt(heathBar.transform.position + heathBar.transform.position - player.transform.position);
    }

    private void Attack()
    {
        Vector3 pos = new Vector3(player.transform.position.x, rotater.position.y, rotater.transform.position.z);
        rotater.LookAt(pos);
        TowerBullet newBullet = Instantiate(bulletPrefab, bulletPoint.transform.position, bulletPoint.transform.rotation, bulletPoint.transform.parent);
        newBullet.SetDamage(attackDamage);
        newBullet.SetPlayer(player);
        newBullet.Fire();
    }


    public void GetDamage(int value)
    {
        if (CurHP - value > 0)
        {
            CurHP -= value;
        }
        else
        {
            CurHP = 0;
            isDead = true;
        }
      
    }


    private void OnColliderTriggerEnter(Collider obj)
    {
        if(obj.tag == "Player")
        {
            playerInRange = true;
        }
    }
    private void OnColliderTriggerExit(Collider obj)
    {
        if (obj.tag == "Player")
        {
            playerInRange = false;
        }
    }

}
