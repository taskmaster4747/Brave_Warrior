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

    public Transform attackPoint;
    public float attackrange = 0.5f;
    public LayerMask enemylayer;

    public int health = 100;
    public Image healthImage;
    public float fallLimit = -10f;

    //private float damageTimer = 0f;
    public float damageInterval = 0.5f; //damage every 0.5 sec

    private SpriteRenderer spriteRenderer;

    private bool isDefending = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        
        //jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isDefending)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
           // isGrounded = false;
        }

        //Defend
        if (Input.GetMouseButton(1))
        {
            isDefending = true;
            animator.SetBool("Defend", true);

            // Stop horizontal movement
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);

        }
        else
        {
            isDefending = false;
            animator.SetBool("Defend", false);

            //normal movement
            float moveInput = Input.GetAxis("Horizontal");
            rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);

            Flip(moveInput);

        }

        //Fall Death
        if(transform.position.y < fallLimit)
        {
            Die();
        }

        healthImage.fillAmount = health / 100f;

        SetAnimation(isDefending ? 0 : Input.GetAxis("Horizontal"));
        //Flip(moveInput);
        combat();
    }

    void FixedUpdate()
    {
        //check if player is Grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
    }

    private void SetAnimation(float moveInput)
    {
        if (isAttacking || isDefending) return;

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

            AttackHit();
        }
    }

    public void EndAttack()
    {
        isAttacking = false;
    }
    
    //Damage
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
    

   public void TakeDamage(int damage)
    {
        if (isDefending) return;

        health -= damage;
        StartCoroutine(BlinkRed());

        if (health <= 0)
        {
            Die();
        }
    }

    /*
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Damage"))
        {
            damageTimer += Time.deltaTime;

            if (damageTimer >= damageInterval)
            {
                TakeDamage(10);
                damageTimer = 0f;
            }
        }
    }
   

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Damage"))
        {
            damageTimer = 0f; //reset when leaving or get off from damage object
        }
    }

     */

    private IEnumerator BlinkRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    private void Die()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    void AttackHit()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackrange,
            enemylayer
            );

        foreach(Collider2D enemy in enemies)
        {
            enemy.GetComponent<Enemy>()?.TakeDamage(20);
        }
    }
    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("DeathZone"))
        {
            Die();
        }
    }
    */
}