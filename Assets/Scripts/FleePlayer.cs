using UnityEngine;

public class FleePlayer : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float fleeRange = 5f;

    Rigidbody2D rb;
    Transform target;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            target = player.transform;
        }
    }

    private void FixedUpdate()
    {
        if (!target)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 myPos = rb.position;
        Vector2 targetPos = target.position;
        float distance = Vector2.Distance(myPos, targetPos);

        if (distance <= fleeRange)
        {
            Flee(myPos, targetPos);
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void Flee(Vector2 myPos, Vector2 targetPos)
    {
        Vector2 direction = (myPos - targetPos).normalized;
        rb.linearVelocity = direction * moveSpeed;

        if (direction != Vector2.zero)
        {
            rb.rotation = Vector2.SignedAngle(Vector2.right, direction);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, fleeRange);
    }
}