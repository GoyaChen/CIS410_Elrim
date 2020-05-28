using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class menuController : MonoBehaviour
{
    [SerializeField] private GameObject meanu = null;
    [SerializeField] private Button topicButton = null;
    [SerializeField] private Button gameButton = null;
    [SerializeField] private Button fullScreenButton = null;
    [SerializeField] private Button exitFullScreenButton = null;
    [SerializeField] private Button quitButton = null;

    public bool isStory;
// Start is called before the first frame update
void Start()
    {
        topicButton.onClick.AddListener(returnTopic);
        gameButton.onClick.AddListener(returnGame);
        fullScreenButton.onClick.AddListener(fullScreen);
        exitFullScreenButton.onClick.AddListener(exitFullScreen);
        quitButton.onClick.AddListener(quitGame);
    }

    private void returnTopic()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void returnGame()
    {
        if (isStory)
        {
            meanu.SetActive(false);
        }
        else
        {
            meanu.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1;
        }

    }

    private void fullScreen()
    {
        Screen.fullScreen = true;
    }

    private void exitFullScreen()
    {
        Screen.fullScreen = false;
    }

    private void quitGame()
    {
        Application.Quit();
    }
}
