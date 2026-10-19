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

    private Vector2 screenSize;
    private Camera Cam;
    public float padding = 0.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");


        Vector3 moveDir = Vector3.Normalize(new Vector2(inputX, inputY));
        rb.linearVelocity = moveDir * sprint();
        //Debug.Log("Speed = " + Speed);
        clampToCamera();
    }

    void clampToCamera()
    {
        float halfHeight = Cam.orthographicSize;
        float halfWidth = halfHeight * Cam.aspect;
        Vector2 camPos = Cam.transform.position;

        float minX = camPos.x - halfWidth + padding;
        float maxX = camPos.x + halfWidth - padding;
        float minY = camPos.y - halfHeight + padding;
        float maxY = camPos.y + halfHeight - padding;

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

}
