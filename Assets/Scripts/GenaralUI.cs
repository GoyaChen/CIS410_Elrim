using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenaralUI : MonoBehaviour
{
    public Button StartGame;
    public Button HowtoPlay;
    public Button Level_1;
    public Button Level_2;
    public Button Level_3;
    [SerializeField] private menuController menu = null;

    private void Start()
    {
        StartGame.onClick.AddListener(runStart);
        HowtoPlay.onClick.AddListener(runHowToPlay);
        Level_1.onClick.AddListener(runLevel_1);
        Level_2.onClick.AddListener(runLevel_2);
        Level_3.onClick.AddListener(runLevel_3);
        menu.gameObject.SetActive(false);
    }

    void Update()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menu.isStory = true;
            menu.gameObject.SetActive(true);
        }
    }

    void runStart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    void runHowToPlay()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(9);
    }

    void runLevel_1()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }

    void runLevel_2()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(4);
    }

    void runLevel_3()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(6);
    }

    
}
