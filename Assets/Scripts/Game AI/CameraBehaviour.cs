using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public float sweepAngle = 45f;
    public float sweepSpeed = 30f;
    public float sweepDelay = 2f;

    public float viewDistance = 6f;
    public float fieldOfView = 60f;
    public LayerMask obstacleMask;

    public bool playerDetected;//field that other scripts can access (used for suspicion when done)

    private float baseAngle;            // rotation the camera starts with
    private float offset;               // current angle relative to baseAngle
    private int direction = 1;          // 1 = sweeping one way, -1 = the other
    private float pauseTimer;
    private Transform player;

    //directoin camera looks
    Vector2 Downward => Quaternion.Euler(0, 0, angleOffset) * transform.right;
    public float angleOffset = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        baseAngle = transform.eulerAngles.z;
        offset = -sweepAngle;   // start at one end so the first sweep is a full one

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p) player = p.transform;
        else Debug.LogWarning("CameraBehaviour: no object tagged 'Player' found.");
    }

    // Update is called once per frame
    void Update()
    {
        Sweep();
        DetectPlayer();
    }


    void Sweep()
    {
        if (pauseTimer > 0f)
        {
            pauseTimer -= Time.deltaTime;//for the delay between sweeps
            return;
        }

        offset += direction * sweepSpeed * Time.deltaTime;

        // reached an end: clamp, flip direction and pause
        if (offset >= sweepAngle)
        {
            offset = sweepAngle;
            direction = -1;
            pauseTimer = sweepDelay;
        }
        else if (offset <= -sweepAngle)
        {
            offset = -sweepAngle;
            direction = 1;
            pauseTimer = sweepDelay;
        }

        transform.rotation = Quaternion.Euler(0f, 0f, baseAngle + offset);//it sets the object’s rotation to its starting angle plus the current sweep offset. In 2D, rotation happens only around the Z axis, so the first two values are 0.
    }

    void DetectPlayer()
    {
        bool seen = false;

        if (player)
        {
            Vector2 toPlayer = (Vector2)player.position - (Vector2)transform.position;
            float dist = toPlayer.magnitude;

            // inside range and inside the cone?
            if (dist <= viewDistance && Vector2.Angle(Downward, toPlayer) <= fieldOfView / 2f)
            {
                // nothing solid between the camera and the player?
                seen = !Physics2D.Raycast(transform.position, toPlayer.normalized, dist, obstacleMask);
            }
        }

        if (seen != playerDetected)
        {
            playerDetected = seen;
            Debug.Log(seen ? "Player spotted!" : "Player lost");
        }
    }

    // Draws the cone in the Scene view (red when the player is spotted)
    void OnDrawGizmos()
    {
        Gizmos.color = playerDetected ? Color.red : Color.yellow;

        float facing = transform.eulerAngles.z + angleOffset;
        float half = fieldOfView / 2f;
        int segments = 20;

        Vector3 prev = transform.position + Quaternion.Euler(0, 0, facing - half) * Vector3.right * viewDistance;
        Gizmos.DrawLine(transform.position, prev);

        for (int i = 1; i <= segments; i++)
        {
            float a = facing - half + (fieldOfView * i / segments);
            Vector3 next = transform.position + Quaternion.Euler(0, 0, a) * Vector3.right * viewDistance;
            Gizmos.DrawLine(prev, next);
            prev = next;
        }
        Gizmos.DrawLine(transform.position, prev);
    }
}
