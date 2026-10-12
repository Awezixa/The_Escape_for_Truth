using UnityEngine;

public class EvadePlayer : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float evadeRange = 5f;
    public float maxLookAheadTime = 2f;

    Rigidbody2D rb;
    Rigidbody2D targetRb;
    Transform target;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            target = player.transform;
            targetRb = player.GetComponent<Rigidbody2D>();
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

        if (distance > evadeRange)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // Predict where the player is heading
        Vector2 targetVelocity = Vector2.zero;
        if (targetRb != null)
        {
            targetVelocity = targetRb.linearVelocity;
        }

        float lookAheadTime = Mathf.Min(distance / Mathf.Max(moveSpeed, 0.01f), maxLookAheadTime);
        Vector2 predictedPos = targetPos + targetVelocity * lookAheadTime;

        // Run away from the predicted position
        Vector2 direction = (myPos - predictedPos).normalized;
        rb.linearVelocity = direction * moveSpeed;

        if (direction != Vector2.zero)
        {
            rb.rotation = Vector2.SignedAngle(Vector2.right, direction);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, evadeRange);
    }
}