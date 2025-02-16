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
    bool moveButton;
    float xDir, running = 0f;

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

        float movementInput = Input.GetAxis("Horizontal");
        HandleMovement(movementInput);
        HandleJump();
        UpdateAnimation();
        Move();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(xDir * speedMovement, rb.linearVelocity.y);
    }

    public void Move()
    {
        if (!moveButton)
            xDir = Input.GetAxisRaw("Horizontal");
        if (xDir > 0)   
            transform.localScale = new Vector3(1, 1, 1);  
        else if (xDir < 0)   
            transform.localScale = new Vector3(-1, 1, 1); 
        if (xDir == 0) return;
        RunSFX();
    }
    public void _Jump()
    {
        if (isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            AudioManager.instance.PlaySFX(AudioManager.instance.JumpSFX);
        }
    }

    public void MoveLeft()
    {
        moveButton = true;
        RunSFX();
        xDir = -1;

    }

    public void MoveRight()
    {
        moveButton = true;
        RunSFX();
        xDir = 1;

    }

    public void moveStop()
    {
        moveButton = false;
        xDir = 0;

    }

     private void HandleMovement(float movementInput) {  
        rb.linearVelocity = new Vector2(movementInput * speedMovement, rb.linearVelocity.y);  
        if (movementInput > 0)   
            transform.localScale = new Vector3(1, 1, 1);  
        else if (movementInput < 0)   
            transform.localScale = new Vector3(-1, 1, 1);  
    }  

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2 (rb.linearVelocity.x, jumpForce);
            AudioManager.instance.PlaySFX(AudioManager.instance.JumpSFX);
        }
        isGrounded = Physics2D.OverlapCircle(GroundCheck.position, 0.2f, GroundLayer);
    }

        public void RunSFX()
    {
        running += Time.deltaTime;
        if (running >= 0.5f)
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.RunSFX);
            running = 0;
        }
    }

    private void UpdateAnimation()
    {
        bool isJumping = !isGrounded;
        bool isRunning = xDir != 0;
        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isJumping", isJumping);
    }

    private void OnCollisionEnter2D(Collision2D other)   
    {  
        if (other.gameObject.CompareTag("Fruits"))  
        {  
            AudioManager.instance.PlaySFX(AudioManager.instance.CollectSFX);
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
            AudioManager.instance.PlaySFX(AudioManager.instance.WinSFX);
            AudioManager.instance.StopBMGMusic();
            winText.SetActive(true);
            Overlay.SetActive(true);
            speedMovement = 0;
        }
    }



}