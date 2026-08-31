using UnityEngine;

public class ObstacleDamage : MonoBehaviour
{
   //  // public int damage = 25;

    public int damage = 25;

    public float damageInterval = 0.5f;
    private float damageTimer = 0f;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            damageTimer += Time.fixedDeltaTime;

            if (damageTimer >= damageInterval)
            {
                PlayerControllerMain player = collision.gameObject.GetComponent<PlayerControllerMain>();

                if (player != null)
                {
                    player.TakeDamage(damage);
                }

                damageTimer = 0f;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            damageTimer = 0f;
        }
    }

}
