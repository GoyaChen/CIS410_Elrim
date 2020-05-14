using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
/// <summary>
/// 玩家角色基类
/// 共有数据
/// </summary>
public class PlayerBase : MonoBehaviour
{
    private CharacterController characterController = null;
   

    [SerializeField]
    [Header("最大生命值")]
    protected int MaxHP = 100;
    [SerializeField]
    [Header("当前生命值")]
    protected int HP = 100; //生命值
    [SerializeField]
    [Header("攻击力")]
    protected int ATK = 10; //攻击力
    [SerializeField]
    [Header("防御力")]
    protected int DF = 5; //防御力
    [SerializeField]
    [Header("魔法攻击")]
    protected bool isMagic = false; //是否为魔法攻击
    [SerializeField]
    [Header("攻击速度")]
    protected float attackSpeed = 500; //攻击速度
    [SerializeField]
    [Header("移动速度")]
    protected float moveSpeed = 0.2f; //移动速度
    [SerializeField]
    [Header("发射力度")]
    protected float fireForce = 1f; //发射力度
    [SerializeField]
    [Header("子弹范围")]
    protected float bulletRange = 1f; //子弹范围

    protected bool isDead = false; //是否已死亡
    protected float characterRotateSpeed = 2.0f; //角色旋转速度
    protected float cameraRotateSpeed = 2.0f; //旋转速度
    protected float lastAttackTime = 0;
    protected float gravity = 1.0f;

    protected Transform cameraTrans = null; //角色摄像机

    protected Vector3 originPostion = Vector3.zero;
    protected Quaternion originRotation = Quaternion.identity;

    #region 公共属性和字段

    public Action OnChangeRed = null; //当触发红色格子

    public Action OnPlayerDead = null; //当玩家死亡
    /// <summary>
    /// 当前血量
    /// </summary>
    public int CurrentHealth { get { return HP; } }
    /// <summary>
    /// 最大血量
    /// </summary>
    public int MaxHealth { get { return MaxHP; } }
    /// <summary>
    /// 是否死亡
    /// </summary>
    public bool Dead { get { return isDead; } }
    /// <summary>
    /// 是否为魔法攻击
    /// </summary>
    public bool IsMagic { get { return isMagic; } }
    #endregion

    #region Unity生命周期
    protected virtual void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        originPostion = transform.position;
        originRotation = transform.rotation;
        characterController = GetComponent<CharacterController>();
        cameraTrans = transform.GetComponentInChildren<Camera>().transform;
        if (cameraTrans == null)
        {
            Debug.LogError("角色下没有摄像机");
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif
        }
    }


    protected virtual void Update()
    {
        if (isDead) return;
        if (characterController.isGrounded == false)
        {
            characterController.Move(-Vector3.up * gravity * Time.deltaTime);
        }
        CameraControl();
        Move();
        Attack();
    }

    //触发检测
    private void OnTriggerEnter(Collider other)
    {
        //如果触发到红色格子
        if (other.tag == "RedWay" && other.GetComponent<MeshRenderer>().material.color == Color.red)
        {
            other.GetComponent<MeshRenderer>().material.color = Color.white;
            OnChangeRed?.Invoke();
        }
    }
    #endregion


    #region 保护和私有方法
    //攻击
    protected virtual void Attack()
    {
    }

  
    //移动和旋转
    protected virtual void Move()
    {
        if (characterController.enabled == false) characterController.enabled = true;
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        characterController.Move((transform.forward * v + transform.right * h) * Time.deltaTime * moveSpeed * 10);
    }
    //摄像机旋转控制
    protected virtual void CameraControl()
    {
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");
        transform.Rotate(transform.up * x * Time.deltaTime * 100 * cameraRotateSpeed, Space.World);
        cameraTrans.Rotate(-cameraTrans.right * y * Time.deltaTime * 100 * cameraRotateSpeed, Space.World);
    }
  
    //死亡
    private void Die()
    {
        isDead = true;
        OnPlayerDead?.Invoke();
    }

    //重置是否已死亡标记
    private void ResetDead()
    {
        isDead = false;
    }

    #endregion


    #region 公有方法
    public void SetCurHP(int hp)
    {
        HP = hp;
    }
    /// <summary>
    /// 受到伤害
    /// </summary>
    /// <param name="value"></param>
    public void GetDamage(int value)
    {
        if (HP == 0) return;
        if (value - DF >= 0) value -= DF;
        else value = 0;

        if (HP - value > 0)
        {
            HP -= value;
        }
        else
        {
            HP = 0;
            Die();
        }
    }

    //重置玩家属性
    public void ResetPlayer()
    {
        //重置位置旋转
        transform.position = originPostion;
        transform.rotation = originRotation;
        //重置摄像机旋转
        cameraTrans.localEulerAngles = Vector3.zero;
        //关闭角色控制器
        characterController.enabled = false;
        //重置血量
        HP = MaxHP;
        //0.2后重置是否已死亡标记
        Invoke("ResetDead", 0.2f);
    }
    #endregion





}
