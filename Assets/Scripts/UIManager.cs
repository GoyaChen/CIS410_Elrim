using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance = null;
    public static UIManager Instance
    {
        get { if (instance == null) instance = FindObjectOfType<UIManager>(); return instance; }
    }

    [SerializeField] private GameObject gameOverUI = null;//游戏结束UI
    [SerializeField] private Text gameOverText = null;
    [SerializeField] private Slider heathBar = null;
    [SerializeField] private Player player = null;
    [SerializeField] private Text playerCountText = null;
    [SerializeField] private GameManager gameManager = null;


    private void Awake()
    {
        gameOverUI.SetActive(false);
        heathBar.maxValue = player.MaxHealth;

    }


    private void Update()
    {
        heathBar.value = player.CurrentHealth;
        playerCountText.text = gameManager.CurPlayerCount.ToString();
    }

    public void ShowGameOver(bool success)
    {
        gameOverText.text = success ? "游戏胜利！" : "游戏失败!";
        gameOverUI.SetActive(true);
    }
}
