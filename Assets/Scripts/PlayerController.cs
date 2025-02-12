using NUnit.Framework;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using TMPro;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{  
    [SerializeField] private float speedMovement = 5f;
    [SerializeField] private float jumpForce = 15f;
    [SerializeField] private LayerMask GroundLayer;
    [SerializeField] private Transform GroundCheck;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] public TextMeshProUGUI scoreText;
    [SerializeField] public GameObject winText;
    [SerializeField] public GameObject Overlay;
    [SerializeField] public Animator animator;
    private bool isGrounded;
    private int score;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Start(){
        winText.SetActive(false);
        Overlay.SetActive(false);
        UpdateScore();
        score = 0;
    }
    void Update(){
        HandleMovement();
        HandleJump();
        UpdateAnimation();
    }

    private void HandleMovement()
    {
        float movementInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(movementInput * speedMovement, rb.linearVelocity.y);
        if (movementInput > 0 ) transform.localScale = new Vector3 (1, 1, 1);
        else if (movementInput < 0) transform.localScale = new Vector3(-1 , 1, 1);
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2 (rb.linearVelocity.x, jumpForce);
        }
        isGrounded = Physics2D.OverlapCircle(GroundCheck.position, 0.2f, GroundLayer);
    }

    private void UpdateAnimation()
    {
        bool isRunning = Mathf.Abs(rb.linearVelocity.x) > 0.1f;
        bool isJumping = !isGrounded;
        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isJumping", isJumping);
    }

    private void OnCollisionEnter2D(Collision2D other)   
    {  
        if (other.gameObject.CompareTag("Fruits"))  
        {  
            Destroy(other.gameObject);  
            score++;  
            UpdateScore();  
            CheckWin();  
        }    
    }

    private void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }

    private void CheckWin()
    {
        if (score >= 24)
        {
            winText.SetActive(true);
            Overlay.SetActive(true);
            speedMovement = 0;
        }
    }



}