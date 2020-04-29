using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int MaxPlayerCount = 3;
    public int CurPlayerCount = 3;
    [SerializeField] private Player player = null;
    [SerializeField] private MeshRenderer[] redCubeMeshRenders = null;

    private void Awake()
    {
        CurPlayerCount = MaxPlayerCount;
        player.OnChangeRed += OnChangeRed;
        player.OnPlayerDead += OnPlayerDead;
        
    }

 
    private void Start()
    {
        player.ResetPlayer();
        CurPlayerCount -= 1;
    }

    private void OnChangeRed()
    {
        if (CurPlayerCount <= 0)
        {
            UIManager.Instance.ShowGameOver(CheckGameSuccess());
        }
    }

    private bool CheckGameSuccess()
    {
        for (int i = 0; i < redCubeMeshRenders.Length; i++)
        {
            if (redCubeMeshRenders[i].material.color == Color.red)
            {
                return false;
            }
        }
        return true;
    }

    private void OnPlayerDead()
    {
        if (CurPlayerCount > 0)
        {
            Invoke("DelayResetPlayer", 3.0f);
        }
        else
        {
            UIManager.Instance.ShowGameOver(false);
        }
     
    }

    private void DelayResetPlayer()
    {
        CurPlayerCount -= 1;
        player.ResetPlayer();
    }

}
