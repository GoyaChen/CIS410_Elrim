using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryController : MonoBehaviour
{
    [SerializeField] private GameObject meanu;
    [SerializeField] private Button topicButton;
    [SerializeField] private Button gameButton;
    public GameObject[] slides;
    public Button Last;
    public Button Next;
    public Button StartGame;
    public int senceID;

    private int count;

    private void Start()
    {
        meanu.SetActive(false);
        topicButton.onClick.AddListener(returnTopic);
        gameButton.onClick.AddListener(returnGame);
        StartGame.onClick.AddListener(runStart);
        Last.onClick.AddListener(GoLast);
        Next.onClick.AddListener(GoNext);
        StartGame.gameObject.SetActive(false);
        count = -1;
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            meanu.SetActive(true);
        }
    }

    void runStart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(senceID);
    }

    void GoLast()
    {
        slides[count].SetActive(false);
        if (count >= 0)
        {
            count--;
        }
    }

    void GoNext()
    {
        if (count < (slides.Length-1))
        {
            count++;
        }
        slides[count].SetActive(true);
    }

    private void returnTopic()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    private void returnGame()
    {
        meanu.SetActive(false);
    }
}
