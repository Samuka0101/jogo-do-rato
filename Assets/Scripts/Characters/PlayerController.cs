using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Dono do script: Tanaka
// Ultima alteracao: 12/01 (Tanaka)

public class PlayerController : MonoBehaviour
{
    #region Enumeradores

    private enum Sounds
    {
        Footstep,
        Jump,
        Hurt
    }

    #endregion

    #region Variaveis

    public float maxSpeed = 10, acceleration = 50, slowdown = 20;
    public float jumpForce = 1050, minHangTime = 0.1f;
    private float horizontalInput;

    private bool walk;
    private int isGrounded = 0;
    private bool jumpInput, isJumping = false;
    private float hangTime = 0;

    private int cheese = 0;
    public int Cheese {
        get { return cheese; }
        set { 
            cheese = value; 
            placar.UpdateCheese(cheese);
        }
    }

    private Vector3 spawnPoint;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator animator;

    private Scoreboard placar;

    private List<GameObject[]> soundsList = new List<GameObject[]>();
    [SerializeField] private GameObject[] footstepSounds;
    [SerializeField] private GameObject[] jumpSounds;
    [SerializeField] private GameObject[] hurtSounds;

    private float footstepCooldown = 0.25f, footstepTimer = 0;

    #endregion

    #region Funcoes de eventos

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        spawnPoint = transform.position;

        placar = GameObject.Find("Scoreboard").GetComponent<Scoreboard>();

        soundsList.Add(footstepSounds);
        soundsList.Add(jumpSounds);
        soundsList.Add(hurtSounds);
    }

    void Update()
    {
        GetInputs();

        Animate();
    }

    void FixedUpdate()
    {
        Move();

        if(isJumping) hangTime += Time.fixedDeltaTime;

        if(jumpInput && isGrounded > 0)
        {
            Jump();
        }
        else if(isJumping && !jumpInput)
        {
            Fall();
        }

        if(rb.velocity.x != 0 && isGrounded > 0)
        {
            footstepTimer -= Time.fixedDeltaTime;

            if(footstepTimer <= 0)
            {
                PlaySound(Sounds.Footstep);
                footstepTimer = footstepCooldown;
            }
        }
    }

    #endregion

    #region Funcoes

    private void GetInputs() 
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        walk = Input.GetKey(KeyCode.LeftShift);

        jumpInput = Input.GetButton("Jump");
    }

    private void Move()
    {
        float velocity = rb.velocity.x;

        // Calcula a velocidade atual com a aplicacao da aceleracao
        if(horizontalInput != 0)
        {
            velocity += horizontalInput * acceleration * Time.fixedDeltaTime;
            
            if(walk && Mathf.Abs(velocity) > maxSpeed/2) velocity = Mathf.Sign(velocity) * maxSpeed/2; // Sua velocidade maxima andando eh metade da normal (correndo)
            else if(Mathf.Abs(velocity) > maxSpeed) velocity = Mathf.Sign(velocity) * maxSpeed; 
        } 
        else if(velocity != 0) 
        {
            velocity -= Mathf.Sign(velocity) * slowdown * Time.fixedDeltaTime;

            if(Mathf.Abs(velocity) < 1) velocity = 0; // Zera a velocidade caso seja muito pequena para evitar micromovimentos
        }

        rb.velocity = new Vector2(velocity, rb.velocity.y);
    }

    private void Jump()
    {
        PlaySound(Sounds.Jump);

        rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.fixedDeltaTime);
        isJumping = true;

        hangTime = 0;
    }

    private void Fall() 
    {
        if(hangTime < minHangTime || rb.velocity.y < 0) return;
        
        rb.velocity = new Vector2(rb.velocity.x, 0);
        isJumping = false;
    }

    private void Animate() {
        if(rb.velocity.x < -0.1) transform.localRotation = Quaternion.Euler(0, 180, 0);
        else if(rb.velocity.x > 0.1) transform.localRotation = Quaternion.Euler(0, 0, 0);

        animator.SetFloat("speedX", Mathf.Abs(rb.velocity.x));
        animator.SetFloat("speedY", rb.velocity.y);
        animator.SetBool("isWalking", walk);
        animator.SetBool("isGrounded", isGrounded > 0);
    }

    public void KillPlayer()
    {
        PlaySound(Sounds.Hurt);

        transform.position = spawnPoint;
    }

    private void PlaySound(Sounds soundType) {
        GameObject[] sounds = soundsList[(int)soundType];
        GameObject sound = sounds[Random.Range(0, sounds.Length)];

        Instantiate(sound, transform.position, Quaternion.identity);
    }

    #endregion

    #region Funcoes de colisao

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            KillPlayer();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Box"))
        {
            if(isGrounded == 0) PlaySound(Sounds.Footstep);

            isGrounded++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if(collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Box"))
        {
            isGrounded--;
        }

        if(collision.gameObject.CompareTag("Checkpoint")) 
        {
            spawnPoint = transform.position;
        }
    }

    #endregion
}
