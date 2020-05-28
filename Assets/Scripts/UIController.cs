using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private static UIController instance = null;
    public static UIController Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<UIController>();
            return instance;
        }
    }

    [SerializeField] private Text timeText = null;
    [SerializeField] private Text targetNumText = null;
    [SerializeField] private Text gameResultText = null;
    [SerializeField] private GameObject selectCharacterObj = null; //选择角色UI对象
    [SerializeField] private GameObject gameOverObj = null; //游戏结束UI对象
    [SerializeField] private Slider heathUI = null; //血条

    private void Awake()
    {
        timeText.text = "";
        targetNumText.text = "0";
        selectCharacterObj.SetActive(false);
        gameOverObj.SetActive(false);
    }


    /// <summary>
    /// 更新游戏时间显示
    /// </summary>
    /// <param name="gameTime"></param>
    public void UpdateGameTime(int gameTime)
    {
        timeText.text = gameTime.ToString();
    }

    /// <summary>
    /// 是否显示选择角色UI
    /// </summary>
    public void SetSelectCharacterUI(bool show)
    {
        selectCharacterObj.SetActive(show);
    }

    /// <summary>
    /// 获取选择角色ui显示状态
    /// </summary>
    /// <returns></returns>
    public bool GetSelectCharacterUIActive()
    {
        return selectCharacterObj.activeSelf;
    }

    /// <summary>
    /// 显示已获取到的目标个数
    /// </summary>
    /// <param name="value">个数</param>
    public void ShowTargetNum(int value)
    {
        targetNumText.text = value.ToString();
    }


    /// <summary>
    /// 显示游戏结果
    /// </summary>
    /// <param name="success">是否胜利</param>
    public void ShowGameOver(bool success)
    {
        gameOverObj.SetActive(true);
        gameResultText.text = success ? "Mission Complete" : "GameOver";
    }

    public void SetHealthBar(int MaxHP, float CurrHP)
    {
        heathUI.maxValue = MaxHP;
        heathUI.value = CurrHP;
    }

}
