using UnityEngine;
using UnityEngine.EventSystems;
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
    Vector2 moveDir;

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

        moveDir = new Vector2(inputX, inputY);
    }

    private void FixedUpdate()
    {

        rb.linearVelocity = moveDir * sprint();
        clampToWorld();
    }


    void clampToWorld()
    {
        Vector2 pos = rb.position;
        Vector2 clamped = new Vector2(
            Mathf.Clamp(pos.x, worldMin.x + padding, worldMax.x - padding),
            Mathf.Clamp(pos.y, worldMin.y + padding, worldMax.y - padding));

        if (clamped != pos) rb.position = clamped;   // only move the player when actually out of bounds
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


}
