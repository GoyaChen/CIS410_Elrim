using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManger : MonoBehaviour
{
    private static GameManger instance = null;
    public static GameManger Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<GameManger>();
            return instance;
        }
    }

    public int requiretarget;//要求的目标数
    [SerializeField] private GameObject targetObjectsParent = null; //所有的目标物体的父对象
    [SerializeField] private float GameTimeLimit = 360; //游戏限制时间 单位s
    [Header("所有的角色，按照1-4的顺序拖入")]
    [SerializeField] private List<PlayerController> allPlayers = null; //所有的角色
    [SerializeField] private Transform birthPoint = null; //出生位置
    [SerializeField] private GameObject transformEffect = null;//传送效果
    [SerializeField] private AudioSource transformAudioStart = null; //传送音效
    [SerializeField] private int senceID;
    [SerializeField] private AudioSource SuccessAudio = null; //成功音效
    [SerializeField] private AudioSource failAudio = null; //失败音效
    [SerializeField] private Slider recoverBar = null; //回血条
    [SerializeField] private GameObject meanu;
    [SerializeField] private Button topicButton;
    [SerializeField] private Button gameButton;

    private int getTargetNum = 0; //获得的目标个数
    private float remainGameTime = 360; //剩余游戏时间
    private GameObject[] allTargetObjects = null; //所有的目标物体
    private PlayerController usingPlayer = null; //正在使用的玩家
    private GameObject tempCamera = null; //临时摄像机，当玩家射线机隐藏时代替显示
    private float timer;
    private float lastime;
    private float recover;
    /// <summary>
    /// 剩余游戏时间
    /// </summary>
    public float RemainGameTime
    {
        get { return remainGameTime; }
    }

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        //菜单初始化
        meanu.SetActive(false);
        topicButton.onClick.AddListener(returnTopic);
        gameButton.onClick.AddListener(returnGame);

        remainGameTime = GameTimeLimit;
        allTargetObjects = new GameObject[targetObjectsParent.transform.childCount];
        for (int i = 0; i < allTargetObjects.Length; i++)
        {
            allTargetObjects[i] = targetObjectsParent.transform.GetChild(i).gameObject;
        }
        //初始隐藏所有角色
        for (int i = 0; i < allPlayers.Count; i++)
        {
            allPlayers[i].gameObject.SetActive(false);
        }
        tempCamera = transform.Find("Camera").gameObject;
        tempCamera.SetActive(false);
        transformAudioStart.Stop();
        lastime = Time.time;
    }


    private void Start()
    {
        usingPlayer = allPlayers[0];
        usingPlayer.transform.position = birthPoint.position;
        usingPlayer.transform.rotation = birthPoint.rotation;
        usingPlayer.gameObject.SetActive(true);
        recoverBar.maxValue = 2000;
    }


    private void Update()
    {
        GameTimeUpdate();
        UpdateCheckTargetObjects();
        SwitchPlayerCharacter();
        UIController.Instance.SetHealthBar(usingPlayer.MaxHealth, usingPlayer.CurrentHealth);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            meanu.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            Time.timeScale = 0;
        }

        recover = (Time.time - lastime) * 20;
        recoverBar.value = recover;

    }

    //切换角色控制
    private void SwitchPlayerCharacter()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            transformEffect.SetActive(true);
            transformEffect.transform.position = usingPlayer.transform.position + new Vector3(0,3,0);
            transformAudioStart.Play();
            if (UIController.Instance.GetSelectCharacterUIActive())
            {
                OnCloseSelectCharacterUI();
            }
            else
            {
                usingPlayer.gameObject.SetActive(false);
                tempCamera.SetActive(true);
                Cursor.lockState = CursorLockMode.Confined;
                UIController.Instance.SetSelectCharacterUI(true);
            }

            if (usingPlayer.currHP + recover < usingPlayer.MaxHealth)
            {
                usingPlayer.currHP += recover;
            }
            else
            {
                usingPlayer.currHP = usingPlayer.MaxHealth;
            }
            lastime = Time.time;

        }
        if (UIController.Instance.GetSelectCharacterUIActive())
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) SelectCharacter(0);
            if (Input.GetKeyDown(KeyCode.Alpha2)) SelectCharacter(1);
            if (Input.GetKeyDown(KeyCode.Alpha3)) SelectCharacter(2);
            if (Input.GetKeyDown(KeyCode.Alpha4)) SelectCharacter(3);
        }
    }

    private void SelectCharacter(int index)
    {
        OnCloseSelectCharacterUI();
        if (index == allPlayers.IndexOf(usingPlayer)) return;
        allPlayers[index].transform.position = usingPlayer.transform.position;
        allPlayers[index].transform.rotation = usingPlayer.transform.rotation;
        usingPlayer.gameObject.SetActive(false);
        usingPlayer = allPlayers[index];
        usingPlayer.gameObject.SetActive(true);
        
    }

    //当关闭选择角色UI
    private void OnCloseSelectCharacterUI()
    {
        UIController.Instance.SetSelectCharacterUI(false);
        tempCamera.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        usingPlayer.gameObject.SetActive(true);
        transformEffect.SetActive(false);
    }


    //持续检测所有的目标物体的显示状态来获取目标已获取的数量
    private void UpdateCheckTargetObjects()
    {
        int temp = 0;
        for (int i = 0; i < allTargetObjects.Length; i++)
        {
            if (!allTargetObjects[i].activeSelf) temp += 1;
        }
        getTargetNum = temp;
        UIController.Instance.ShowTargetNum(getTargetNum);
        if(getTargetNum >= requiretarget)
        {
            UIController.Instance.ShowGameOver(true);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            SuccessAudio.Play();
            timer += Time.deltaTime;
            if(timer > 5.0f)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(senceID);
            }
        }
    }


    //游戏时间更新并判断游戏结束条件
    private void GameTimeUpdate()
    {
        remainGameTime -= Time.deltaTime; //游戏时间减少
        if (remainGameTime <= 0)
        {
            remainGameTime = 0;
            UIController.Instance.ShowGameOver(false);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            failAudio.Play();
            timer += Time.deltaTime;
            if (timer > 5.0f)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            }

        }
        UIController.Instance.UpdateGameTime(Mathf.RoundToInt(remainGameTime)); //更新剩余游戏时间显示
    }

    private void returnTopic()
    {
        Cursor.lockState = CursorLockMode.Confined;
        //UIController.Instance.SetSelectCharacterUI(true);
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    private void returnGame()
    {
        meanu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
    }

}
