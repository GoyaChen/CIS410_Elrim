using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera Camera = null;
    [SerializeField] private Transform DamagerPoint = null;//攻击器产生点（群攻时使用）
    [SerializeField] private GameObject Damager = null;//攻击器（群攻时使用）
    [SerializeField] private Transform bulletPoint = null; //子弹点
    [SerializeField] private GameObject bulletPrefab = null; //炮弹
    [SerializeField] private ParticleSystem fireEffect = null;//开火效果
    [SerializeField] private AudioSource fireAudio = null;//开火音效
    [SerializeField] private ParticleSystem deadEffect = null;//死亡效果
    [SerializeField] private AudioSource deadAudio = null;//死亡音效
    [SerializeField] private GameObject notification = null;//提示

    [Header("基础数据")]
    [SerializeField]
    public int maxHP;//最大HP
    public int defense;//防御力
    public float moveSpeed;//移动速度
    public float rotateSpeed;//转身速度
    public float attackSpeed; //攻击速度
    public int attackDamage;//伤害值
    public int explodeDamage = 0;//爆炸伤害
    public int conDamage = 0;//持续伤害
    public float fieldOfFire;//射程
    public float bulletSpeed;//子弹速度
    public bool ismagic;//是否是魔法攻击
    public float firstAttactDelay;
    public float mouseSensitivity = 100;

    public float currHP;
    private bool isDead;
    private float lastAttackTime = 0;
    private bool isWalking;
    private Animator m_Animator;
    private Rigidbody m_Rigidbody;
    private Vector3 m_Movement;
    private CharacterController characterController = null;
    private float timer;
    private float normalMoveSpeed;
    private float aimMoveSpeed;

    public float CurrentHealth { get { return currHP; } }
    public int MaxHealth { get { return maxHP; } }
    public bool Dead { get { return isDead; } }
    public Vector3 CurrPostition { get { return transform.position; } }

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        characterController = GetComponent<CharacterController>();
        currHP = maxHP;
        isDead = false;
        bulletPointRotate();
        fireEffect.gameObject.SetActive(false);
        normalMoveSpeed = moveSpeed;
        aimMoveSpeed = moveSpeed / 3;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (currHP == 0)
        {
            Die();
        }
        else
        {
            isDead = false;
        }
        if (gameObject.activeSelf == false)
        {
            currHP += 1 * Time.deltaTime;
        }
        if (characterController.isGrounded == false)
        {
            characterController.Move(-Vector3.up * 1.0f * Time.deltaTime);
        }
        CameraControl();
        Move();
        if(bulletPoint != null && bulletPrefab != null) bulletPointRotate();

        if (Input.GetMouseButton(1))
        {
            m_Animator.SetBool("Aiming", true);
            m_Animator.SetFloat("Speed", 0.0f);
            moveSpeed = aimMoveSpeed;
            Camera.fieldOfView = 60;
        }
        else
        {
            m_Animator.SetBool("Aiming", false);
            moveSpeed = normalMoveSpeed;
            Camera.fieldOfView = 90;
        }

        
        if (Input.GetMouseButtonDown(0))
        {
            if (m_Animator.GetBool("Aiming"))
            {
                Attack();
            }
            else
            {
                GameObject Note = Instantiate(notification);
                Note.SetActive(true);
                Destroy(Note, 1.0f);
            }
           
        }
        /*    
        if (m_Animator.GetBool("Aiming"))
        {
            if (isfirstAttact)
            {
                timer += Time.deltaTime;
                if (timer > firstAttactDelay)
                {
                    Attack();
                    isfirstAttact = false;
                    timer = 0;
                }
            }
            else
            {
                Attack();
            }

        }
        else
        {
            isfirstAttact = true;
        }
        */
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "target")
        {
            other.gameObject.SetActive(false);
        }
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
                currHP -= (((value - defense) >= 0) ? (value - defense) : 0);
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
        isWalking = hasHorizontalInput || hasVerticalInput;
        if (isWalking)
        {
            m_Animator.SetFloat("Speed", moveSpeed);
        }
        else
        {
            m_Animator.SetFloat("Speed", 0.0f);
        }

        characterController.Move((transform.forward * v + transform.right * h) * Time.deltaTime * moveSpeed*3);
    }
    //摄像机旋转控制
    protected virtual void CameraControl()
    {
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");
        transform.Rotate(transform.up * x * Time.deltaTime * mouseSensitivity * rotateSpeed, Space.World);
        Camera.gameObject.transform.Rotate(-Camera.gameObject.transform.right * y * Time.deltaTime * mouseSensitivity * rotateSpeed, Space.World);
    }

    private void bulletPointRotate()
    {
        Ray ray = new Ray(Camera.gameObject.transform.position, Camera.gameObject.transform.forward);
        Vector3 point = ray.GetPoint(10.0f);
        bulletPoint.LookAt(point);
    }

    private void Attack()
    {
        if (Time.time - lastAttackTime >= attackSpeed)
        {
            lastAttackTime = Time.time;
            m_Animator.SetTrigger("Attack");
            if (fireEffect != null)
            {
                fireEffect.gameObject.SetActive(true);
                fireEffect.Play();
            }

            if (fireAudio != null) fireAudio.Play();
            if (bulletPoint != null && bulletPrefab != null)
            {
                GameObject newBulletPrefeb = Instantiate(bulletPrefab, bulletPoint.position, bulletPoint.rotation, bulletPoint.transform.parent);
                newBulletPrefeb.SetActive(true);
                BulletController newBullet = newBulletPrefeb.GetComponent<BulletController>();
                newBullet.SetDamage(attackDamage);
                newBullet.SetisDamageMagic(ismagic);
                newBullet.SetExplodeDamage(explodeDamage);
                newBullet.SetConDamage(conDamage);
                newBullet.SetmovefieldOfFire(fieldOfFire);
                newBullet.SetmoveSpeed(bulletSpeed);
                newBullet.SetselfCollider(this.GetComponent<CharacterController>());
                newBullet.tag = "PlayerBullet";
                newBullet.Fire();
            }
            else if(Damager != null)
            {
                
                GameObject newDamager = Instantiate(Damager, DamagerPoint.position, DamagerPoint.rotation, DamagerPoint.parent);
                newDamager.SetActive(true);
                DamagerControler damagerControler = newDamager.GetComponent<DamagerControler>();
                damagerControler.SetDamage(attackDamage);
                damagerControler.SetisMagic(ismagic);
                damagerControler.SetConDamage(conDamage);
                damagerControler.SetselfCollider(this.GetComponent<CharacterController>());
                newDamager.tag = "PlayerBullet";
            }
        }
    }

    private void Die()
    {
        isDead = true;
        m_Animator.SetTrigger("Death");
        deadAudio.Play();
        deadEffect.Play();
        timer += Time.deltaTime;
        if (timer > 3.0f) gameObject.SetActive(false);
    }

    public void SetcurHP(int currHP)
    {
        this.currHP = currHP;
    }
}
