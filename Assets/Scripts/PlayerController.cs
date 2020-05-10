using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public Text countText;
    public Text winText;
    public float jumpforce;

    private Rigidbody rb;
    private int count;
    private int num_jump;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        winText.text = "";
        num_jump = 0;
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        rb.AddForce(movement * speed);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (num_jump < 1)
            {
                rb.AddForce(new Vector3(0.0f, jumpforce, 0.0f));
                num_jump++;
            }
        }

        if (this.transform.position.y == 0.5)
        {
            num_jump = 0;
        }

        if (this.transform.position.y <= -50)
        {
            SceneManager.LoadScene(0);
        }
        
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Pick Up"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
        }
        
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if (count >= 12)
        {
            winText.text = "You Win!";
        }
    }
}
