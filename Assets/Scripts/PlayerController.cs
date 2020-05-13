using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject Camera = null;
    [SerializeField] private Transform DamagerPoint = null;//攻击器产生点（群攻时使用）
    [SerializeField] private DamagerControler Damager = null;//攻击器（群攻时使用）
    [SerializeField] private Transform bulletPoint = null; //子弹点
    [SerializeField] private BulletController bulletPrefab = null; //炮弹
    [SerializeField] private ParticleSystem fireEffect = null;//开火效果
    [SerializeField] private AudioSource fireAudio = null;//开火音效
    [SerializeField] private ParticleSystem deadEffect = null;//死亡效果
    [SerializeField] private AudioSource deadAudio = null;//死亡音效

    [Header("基础数据")]
    [SerializeField]
    public int maxHP;//最大HP
    public int defense;//防御力
    public float moveSpeed;//移动速度
    public float rotateSpeed;//转身速度
    public float attackSpeed; //攻击速度
    public int attackDamage;//伤害值
    public int explodeDamage = 0;//爆炸伤害
    public float fieldOfFire;//射程
    public float bulletSpeed;//子弹速度
    public bool ismagic;//是否是魔法攻击

    public int currHP;
    private bool isDead;
    private float lastAttackTime = 0;
    private bool isWalking;
    private Animator m_Animator;
    private Rigidbody m_Rigidbody;
    private Vector3 m_Movement;
    private CharacterController characterController = null;

    public int CurrentHealth { get { return currHP; } }
    public int MaxHealth { get { return maxHP; } }
    public bool Dead { get { return isDead; } }

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        characterController = GetComponent<CharacterController>();
        currHP = maxHP;
        isDead = false;
        bulletPointRotate();
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) return;
        if (characterController.isGrounded == false)
        {
            characterController.Move(-Vector3.up * 1.0f * Time.deltaTime);
        }
        CameraControl();
        Move();
        bulletPointRotate();
        Attack();
    }

    public void GetDamage(int value, bool isDamageMagic)
    {
        if (currHP == 0) return;
        if (currHP - value > 0)
        {
            if (isDamageMagic)
            {
                currHP -= value;
            }
            else
            {
                currHP -= (value - defense);
            }
        }
        else
        {
            currHP = 0;
            Die();
        }
    }

    protected virtual void Move()
    {
        if (characterController.enabled == false) characterController.enabled = true;
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        bool hasHorizontalInput = !Mathf.Approximately(h, 0f);
        bool hasVerticalInput = !Mathf.Approximately(v, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;
        if (isWalking)
        {
            m_Animator.SetBool("Aiming", false);
            m_Animator.SetFloat("Speed", moveSpeed);
        }
        else
        {
            m_Animator.SetFloat("Speed", 0.0f);
            m_Animator.SetBool("Aiming", true);
        }

        characterController.Move((transform.forward * v + transform.right * h) * Time.deltaTime * moveSpeed*3);
    }
    //摄像机旋转控制
    protected virtual void CameraControl()
    {
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");
        transform.Rotate(transform.up * x * Time.deltaTime * 100 * rotateSpeed, Space.World);
        Camera.transform.Rotate(-Camera.transform.right * y * Time.deltaTime * 100 * rotateSpeed, Space.World);
    }

    private void bulletPointRotate()
    {
        Ray ray = new Ray(Camera.transform.position, Camera.transform.forward);
        Vector3 point = ray.GetPoint(10.0f);
        bulletPoint.LookAt(point);
    }

    private void Attack()
    {
        if (Input.GetMouseButtonDown(0) && Time.time - lastAttackTime >= attackSpeed)
        {
            lastAttackTime = Time.time;
            if (fireEffect != null) fireEffect.Play();
            if (fireAudio != null) fireAudio.Play();
            if (bulletPoint != null && bulletPrefab != null)
            {
                BulletController newBullet = Instantiate(bulletPrefab, bulletPoint.position, bulletPoint.rotation, bulletPoint.transform.parent);
                newBullet.SetDamage(attackDamage);
                newBullet.SetisDamageMagic(ismagic);
                newBullet.SetExplodeDamage(explodeDamage);
                newBullet.SetmovefieldOfFire(fieldOfFire);
                newBullet.SetmoveSpeed(bulletSpeed);
                newBullet.tag = "PlayerBullet";
                newBullet.Fire();
            }
            else if(Damager != null)
            {
                DamagerControler newDamager = Instantiate(Damager, DamagerPoint.position, DamagerPoint.rotation, DamagerPoint.parent);
                newDamager.SetDamage(attackDamage);
                newDamager.SetisMagic(ismagic);
                newDamager.tag = "PlayerBullet";
            }
            
        }
    }

    private void Die()
    {
        isDead = true;
    }
}
