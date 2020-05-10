﻿using System;
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
    public float fieldOfFire;//射程
    public float moveSpeed;//子弹速度
    public bool ismagic;//是否是魔法攻击

    private Player player = null;
    private bool playerInRange;
    private int CurHP;
    private float lastAttackTime = 0;
    private bool isDead = false;
    private Vector3 _angles;


    void Awake()
    {
        CurHP = MaxHP;
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        heathUI.maxValue = MaxHP;
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
            }
        }
        heathUI.value = CurHP;
        heathBar.LookAt(heathBar.transform.position + heathBar.transform.position - player.transform.position);
    }

    private void Attack()
    {

        TowerBullet newBullet = Instantiate(bulletPrefab, bulletPoint.transform.position, bulletPoint.transform.rotation, bulletPoint.transform.parent);
        newBullet.SetDamage(attackDamage);
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
}