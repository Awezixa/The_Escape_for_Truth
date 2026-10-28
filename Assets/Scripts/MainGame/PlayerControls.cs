using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{

    public float Speed;
    public float inputX;
    public float inputY;
    public float input;
    private Rigidbody2D rb;
    public float sprintSpeed;
    public Vector2 facing = Vector2.right;

    //for clamping into world bounds
    [Header("World Bounds")]
    public Vector2 worldMin = new Vector2(-20f, -20f);//bottom left corner of play area
    public Vector2 worldMax = new Vector2(20f, 20f);//top right corner of the play area


    private Vector2 screenSize;
    public float padding = 0.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");

        if (inputX != 0 || inputY != 0)
        {
            facing = new Vector2(inputX, inputY).normalized;
        }

        Vector3 moveDir = Vector3.Normalize(new Vector2(inputX, inputY));
        rb.linearVelocity = moveDir * sprint();
        //Debug.Log("Speed = " + Speed);
        clampToWorld();
    }

    void clampToWorld()
    {

        float minX = worldMin.x + padding;
        float maxX = worldMax.x - padding;
        float minY = worldMin.y + padding;
        float maxY = worldMax.y - padding;

        //clamp the players position within cameras zone
        Vector2 pos = rb.position;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        rb.position = pos;
    }


    float sprint()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            return Speed * sprintSpeed;
            
        }
        return Speed;
    }

    //draw world boundries gizmo
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector2 center = (worldMin + worldMax) / 2f;
        Vector2 size = worldMax - worldMin;
        Gizmos.DrawWireCube(center, size);
    }


    void work()
    {

    }

}
