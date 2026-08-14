using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;
    public Transform[] points;

    private int i;
    private SpriteRenderer spriteRenderer;

    public int health = 50;

    //public LayerMask enemylayer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if(Vector2.Distance(transform.position, points[i].position) < 0.25f)
        {
            i++;
            if(i == points.Length)
            {
                i = 0;
            }
        }

        transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);

        spriteRenderer.flipX = (transform.position.x - points[i].position.x) < 0f;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log($"Enemy hit! Health: {health}");

        GetComponent<SpriteRenderer>().color = Color.red;
        Invoke("ResetColor", 0.1f);

        if(health <= 0)
        {
            Die();
        }
    }

    void ResetColor()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
