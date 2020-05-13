using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 玩家
/// </summary>
public class Player : MonoBehaviour
{
    private CharacterController characterController = null;
    [SerializeField] private Transform cameraTrans = null;
    [SerializeField] private Transform wayPointsParent = null; //路径
    [SerializeField] private Transform gunTrans = null;
    [SerializeField] private Transform bulletPoint = null;

    public Action OnChangeRed = null;
    public Action OnPlayerDead = null;


    [SerializeField]
    [Header("最大生命值")]
    private int MaxHP = 100;
    [SerializeField]
    [Header("当前生命值")]
    private int HP = 100; //生命值
    private bool isDead = false; //是否已死亡
    private int ATK = 10; //攻击力
    private float attackSpeed = 500; //攻击速度
    public int dafense;

    private float lastAttackTime = 0;

    private int curWayPointIndex = 0;
    private Transform targetPoint = null;

    private float moveSpeed = 0.2f; //移动速度
    private float characterRotateSpeed = 2.0f; //角色旋转速度
    private float cameraRotateSpeed = 2.0f; //旋转速度

    private float gravity = 1.0f;

    private Vector3 originPostion = Vector3.zero;
    private Quaternion originRotation = Quaternion.identity;

    public int CurrentHealth { get { return HP; } }
    public int MaxHealth { get { return MaxHP; } }

    public bool Dead { get { return isDead; } }


  
    private void Awake()
    {
        originPostion = transform.position;
        originRotation = transform.rotation;
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        targetPoint = wayPointsParent.GetChild(curWayPointIndex + 1);
    }


    private void Update()
    {
        if (isDead) return;
        if (characterController.isGrounded == false)
        {
            characterController.Move(-Vector3.up * gravity * Time.deltaTime);
        }
        CameraControl();
        //MoveWithWayPoints();
        MoveAndRotate();
        GunControl();
        Attack();
    }

    private void Attack()
    {
        if (Input.GetMouseButtonDown(0) && Time.time - lastAttackTime >= 100 / attackSpeed)
        {
            lastAttackTime = Time.time;
        }
    }

    private void MoveAndRotate()
    {
        if (characterController.enabled == false) characterController.enabled = true;
        characterController.Move(transform.forward * Time.deltaTime * moveSpeed * 10);
        float h = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up * h * Time.deltaTime * 100 * characterRotateSpeed);
    }

    private void CameraControl()
    {
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");
        cameraTrans.Rotate((transform.up * x - cameraTrans.right * y) * Time.deltaTime * 100 * cameraRotateSpeed, Space.World);
    }

    private void MoveWithWayPoints()
    {
        if (Vector3.Distance(transform.position, targetPoint.position) > 2.5f)
        {
            Vector3 pos = new Vector3(targetPoint.position.x, transform.position.y, targetPoint.position.z);
            Vector3 dir = (pos - transform.position).normalized;
            characterController.Move(dir * moveSpeed * Time.deltaTime * 10);
        }
        else
        {
            if (curWayPointIndex + 1 < wayPointsParent.childCount)
            {
                curWayPointIndex++;
                targetPoint = wayPointsParent.GetChild(curWayPointIndex);
            }
        }
    }

    

    private void GunControl()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 point = ray.GetPoint(3);
        gunTrans.LookAt(point);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "RedWay" && other.GetComponent<MeshRenderer>().material.color == Color.red)
        {
            other.GetComponent<MeshRenderer>().material.color = Color.white;
            OnChangeRed?.Invoke();
        }
    }


    public void GetDamage(int value, bool isDamageMagic)
    {
        if (HP == 0) return;
        if (HP - value > 0)
        {
            if (isDamageMagic)
            {
                HP -= value;
            }
            else
            {
                HP -= (value-dafense);
            }
            
        }
        else
        {
            HP = 0;
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        OnPlayerDead?.Invoke();
    }

    public void ResetPlayer()
    {
        transform.position = originPostion;
        transform.rotation = originRotation;
        cameraTrans.localEulerAngles = Vector3.zero;
        characterController.enabled = false;
        HP = MaxHP;
        Invoke("ResetDead", 0.2f);
    }

    private void ResetDead()
    {
        isDead = false;
    }

}
