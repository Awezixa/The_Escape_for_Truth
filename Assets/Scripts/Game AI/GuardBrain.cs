using UnityEngine;

public class GuardBrain : MonoBehaviour
{
    public float viewDistance = 6f;
    public float fieldOfView = 70f;      // total cone angle in degrees
    public LayerMask obstacleMask;       // tick Walls

    [Range(0f, 1f)] public float chaseThreshold = 0.7f;   // suspicion needed before this guard chases
    public float catchDistance = 0.5f;                    // how close counts as caught

    [SerializeField] WaypointFollow patrol;
    [SerializeField] PursuePlayer pursue;

    Transform player;
    SuspicionMeter meter;
    Vector2 lastPos;

    void Start()
    {
        if (!patrol) patrol = GetComponent<WaypointFollow>();//to check and make sure guard is patrolling
        if (!pursue) pursue = GetComponent<PursuePlayer>();//to change to pursue when player has enough suspicion

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p) player = p.transform;

        meter = FindFirstObjectByType<SuspicionMeter>();
        if (meter == null) Debug.LogWarning("GuardBrain: no SuspicionMeter found in the scene.");

        lastPos = transform.position;
    }

    void Update()
    {
        if (!player || meter == null) return;
        if (GameManager.Instance.IsGameOver) return;

        Vector2 pos = transform.position;

        // while patrolling, face the way we're walking (PursuePlayer rotates us while chasing)
        if (!pursue.inChase)
        {
            Vector2 delta = pos - lastPos; //calc distance from pursue position to original follow position
            if (delta.sqrMagnitude > 0.00001f)
                transform.rotation = Quaternion.Euler(0f, 0f, Vector2.SignedAngle(Vector2.right, delta));
        }
        lastPos = pos;

        // start the chase: suspicious enough AND the player is in our cone
        if (!pursue.inChase && meter.suspicion >= chaseThreshold && CanSeePlayer())
        {
            pursue.inChase = true;
            Debug.Log(name + " is pursuing the player!");
        }

        // only one of the two movement scripts runs at a time
        patrol.enabled = !pursue.inChase;

        // caught
        if (pursue.inChase && Vector2.Distance(pos, player.position) <= catchDistance)
        {
            GameManager.Instance.PlayerCaught();
        }
    }

    bool CanSeePlayer()
    {
        Vector2 toPlayer = (Vector2)player.position - (Vector2)transform.position;//distance between player and guard
        float dist = toPlayer.magnitude;

        if (dist > viewDistance) return false;
        if (Vector2.Angle(transform.right, toPlayer) > fieldOfView / 2f) return false;

        // a wall between us blocks the view
        return !Physics2D.Raycast(transform.position, toPlayer.normalized, dist, obstacleMask);
    }

    // select the guard in the Scene view to see its cone edges
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 left = Quaternion.Euler(0, 0, fieldOfView / 2f) * transform.right * viewDistance;
        Vector3 right = Quaternion.Euler(0, 0, -fieldOfView / 2f) * transform.right * viewDistance;
        Gizmos.DrawLine(transform.position, transform.position + left);
        Gizmos.DrawLine(transform.position, transform.position + right);
    }
}