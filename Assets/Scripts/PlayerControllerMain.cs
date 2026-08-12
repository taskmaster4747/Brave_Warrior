using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerControllerMain : MonoBehaviour
{

    public float speed = 5f;
    public float jumpForce = 5f;

    private Rigidbody2D rb;
    private Vector2 movement;

    private bool isGrounded;

    public Transform groundCheck;
    public float groundRadius = 0.2f;
    public LayerMask groundLayer;

    private Animator animator;

    private bool facingRight = true;

    private bool isAttacking = false;

    public int health = 100;
    public Image healthImage;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);


        //jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
           // isGrounded = false;
        }

        healthImage.fillAmount = health / 100f;

        SetAnimation(moveInput);
        Flip(moveInput);
        combat();
    }

    void FixedUpdate()
    {
        //check if player is Grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
    }

    private void SetAnimation(float moveInput)
    {
        if (isAttacking) return;

        if (isGrounded)
        {
            if (moveInput == 0)
            {
                animator.Play("Idle");
            }
            else
            {
                animator.Play("Run");
            }
        }
        else
        {
            if(rb.linearVelocity.y > 0)
            {
                animator.Play("Jump");
            }
            else
            {
                animator.Play("Fall");
            }
        }
    }

    void Flip(float moveInput)
    {
        if(moveInput > 0 && !facingRight)
        {
            facingRight = true;
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (moveInput < 0 && facingRight)
        {
            facingRight = false;
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    void combat()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isAttacking = true;
           animator.SetTrigger("Attack");
        }
    }

    public void EndAttack()
    {
        isAttacking = false;
    }

   private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Damage")
        {
            health -= 25;
            StartCoroutine(BlinkRed());

            if (health <= 0)
            {
                Die();
            }
        }
    }

    private IEnumerator BlinkRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    private void Die()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level1");
    }
}