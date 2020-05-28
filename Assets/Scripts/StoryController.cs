using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryController : MonoBehaviour
{
    [SerializeField] private menuController menu = null;
    public GameObject[] slides;
    public Button Last = null;
    public Button Next = null;
    public Button StartGame = null;
    public int senceID;

    private int count;

    private void Start()
    {
        menu.gameObject.SetActive(false);
        StartGame.onClick.AddListener(runStart);
        Last.onClick.AddListener(GoLast);
        Next.onClick.AddListener(GoNext);
        StartGame.gameObject.SetActive(false);
        count = -1;
    }

    // Update is called once per frame
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
        UnityEngine.SceneManagement.SceneManager.LoadScene(senceID);
    }

    void GoLast()
    {
        slides[count].SetActive(false);
        if (count > 0)
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
}
