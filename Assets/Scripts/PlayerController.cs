using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    public float damping = 0.95f;
    private int count;
    private float movementX;
    private float movementY;
    public float speed = 0;
    private bool canMove;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    public GameObject loseTextObject;
    public GameObject restartButton;

    void Start()
    {
        winTextObject.SetActive(false);
        loseTextObject.SetActive(false);
        restartButton.SetActive(false);
        count = 0;
        rb = GetComponent<Rigidbody>();
        SetCountText();
        canMove = true;
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            Vector3 movement = new Vector3(movementX, 0.0f, movementY);
            rb.AddForce(movement * speed);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            Renderer wallRenderer = collision.gameObject.GetComponent<Renderer>();
            wallRenderer.material.color = Color.red;
            loseTextObject.SetActive(true);
            restartButton.SetActive(true);
            canMove = false;
            StopBall();
        }
    }

    void OnMove(InputValue movementValue)
    {
        if (!loseTextObject.activeSelf)
        {
            Vector2 movementVector = movementValue.Get<Vector2>();
            movementX = movementVector.x;
            movementY = movementVector.y;
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if (count >= 5)
        {
            winTextObject.SetActive(true);
            restartButton.SetActive(true);
            canMove = false;
            StopBall();
        }
    }

    public void ResetTheGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        canMove = true; // Set canMove to true for the new game
        SetCountText(); // Reset the count and UI text
    }

    private void StopBall()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
