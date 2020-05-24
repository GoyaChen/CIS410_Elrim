using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public bool chaosModel;
    [SerializeField] private GameObject targetObjectsParent = null; //所有的目标物体的父对象
    [SerializeField] private float GameTimeLimit = 360; //游戏限制时间 单位s
    [Header("所有的角色，按照1-4的顺序拖入")]
    [SerializeField] private List<PlayerController> allPlayers = null; //所有的角色
    [SerializeField] private Transform birthPoint = null; //出生位置

    private int getTargetNum = 0; //获得的目标个数
    private float remainGameTime = 360; //剩余游戏时间
    private GameObject[] allTargetObjects = null; //所有的目标物体
    private PlayerController usingPlayer = null; //正在使用的玩家
    private GameObject tempCamera = null; //临时摄像机，当玩家射线机隐藏时代替显示
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
    }


    private void Start()
    {
        usingPlayer = allPlayers[0];
        usingPlayer.transform.position = birthPoint.position;
        usingPlayer.transform.rotation = birthPoint.rotation;
        usingPlayer.gameObject.SetActive(true);
    }


    private void Update()
    {
        GameTimeUpdate(); 
        UpdateCheckTargetObjects();
        SwitchPlayerCharacter();

        UIController.Instance.SetHealthBar(usingPlayer.MaxHealth, usingPlayer.CurrentHealth);
    }

    //切换角色控制
    private void SwitchPlayerCharacter()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (UIController.Instance.GetSelectCharacterUIActive())
            {
                OnCloseSelectCharacterUI();
            }
            else
            {
                Time.timeScale = 0;
                usingPlayer.gameObject.SetActive(false);
                Transform camTrans = usingPlayer.GetComponentInChildren<Camera>().transform;
                tempCamera.transform.position = camTrans.position;
                tempCamera.transform.rotation = camTrans.rotation;
                tempCamera.SetActive(true);
                UIController.Instance.SetSelectCharacterUI(true);
            }
          
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
        Time.timeScale = 1;
        UIController.Instance.SetSelectCharacterUI(false);
        tempCamera.SetActive(false);
        usingPlayer.gameObject.SetActive(true);
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
        }
    }


    //游戏时间更新并判断游戏结束条件
    private void GameTimeUpdate()
    {
        remainGameTime -= Time.deltaTime; //游戏时间减少
        if (remainGameTime <= 0)
        {
            remainGameTime = 0;
            Time.timeScale = 0;
            UIController.Instance.ShowGameOver(false);
        }
        UIController.Instance.UpdateGameTime(Mathf.RoundToInt(remainGameTime)); //更新剩余游戏时间显示
    }


}
