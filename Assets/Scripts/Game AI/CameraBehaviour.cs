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


    //camera gizmos visible to player
    public Material coneMaterial;      // optional, see the note below
    public int coneSegments = 30;      // more = smoother edge
    public int coneSortingOrder = 0;   // keep this below the player's Order in Layer
    public Color normalColour = new Color(1f, 1f, 0f, 0.25f);
    public Color alertColour = new Color(1f, 0f, 0f, 0.35f);

    private Mesh coneMesh;
    private MeshRenderer coneRenderer;
    private GameObject coneObject;



    //directoin camera looks
    Vector2 forward => Quaternion.Euler(0, 0, angleOffset) * transform.right;
    public float angleOffset = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        baseAngle = transform.eulerAngles.z;
        offset = -sweepAngle;   // start at one end so the first sweep is a full one

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p) player = p.transform;
        else Debug.LogWarning("CameraBehaviour: no object tagged 'Player' found.");
        obstacleMask = LayerMask.GetMask("Objects", "Throwable");
    }

    // Update is called once per frame
    void Update()
    {
        Sweep();
        DetectPlayer();
    }
    // LateUpdate runs after Update, so the cone follows this frame's sweep rotation
    //void LateUpdate()
    //{
    //    DrawCone();
    //}


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
            //GameManager.Instance.playerSus += 2;
            //Debug.Log("PLayer sus: ");


            // inside range and inside the cone?
            if (dist <= viewDistance && Vector2.Angle(forward, toPlayer) <= fieldOfView / 2f)
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

    //void CreateCone()
    //{
    //    // a separate, unparented object, so this camera's scale and rotation can't distort the mesh
    //    coneObject = new GameObject(name + "_VisionCone");
    //    coneMesh = new Mesh();
    //    coneObject.AddComponent<MeshFilter>().mesh = coneMesh;
    //    coneRenderer = coneObject.AddComponent<MeshRenderer>();

    //    coneRenderer.sharedMaterial = coneMaterial != null
    //        ? coneMaterial
    //        : new Material(Shader.Find("Sprites/Default"));   // unlit, supports transparency and vertex colour

    //    coneRenderer.sortingOrder = coneSortingOrder;
    //}



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

    //void DrawCone()
    //{
    //    int n = Mathf.Max(2, coneSegments);
    //    Vector3[] verts = new Vector3[n + 2];
    //    Color[] colours = new Color[n + 2];
    //    int[] tris = new int[n * 3];

    //    Vector2 origin = transform.position;
    //    float facing = transform.eulerAngles.z + angleOffset;   // same direction the detection uses
    //    float start = facing - fieldOfView / 2f;
    //    Color col = playerDetected ? alertColour : normalColour;

    //    verts[0] = origin;
    //    colours[0] = col;

    //    for (int i = 0; i <= n; i++)
    //    {
    //        float a = (start + fieldOfView * i / n) * Mathf.Deg2Rad;
    //        Vector2 dir = new Vector2(Mathf.Cos(a), Mathf.Sin(a));

    //        // shorten this edge if a wall is in the way
    //        float dist = viewDistance;
    //        RaycastHit2D hit = Physics2D.Raycast(origin, dir, viewDistance, obstacleMask);
    //        if (hit.collider != null) dist = hit.distance;

    //        verts[i + 1] = origin + dir * dist;
    //        colours[i + 1] = col;
    //    }

    //    for (int i = 0; i < n; i++)
    //    {
    //        tris[i * 3] = 0;
    //        tris[i * 3 + 1] = i + 2;   // clockwise winding so the front face points at the 2D camera
    //        tris[i * 3 + 2] = i + 1;
    //    }

    //    coneMesh.Clear();
    //    coneMesh.vertices = verts;
    //    coneMesh.colors = colours;
    //    coneMesh.triangles = tris;
    //    coneMesh.RecalculateBounds();
    //}

    //void OnDestroy()
    //{
    //    if (coneObject) Destroy(coneObject);
    //}












}
