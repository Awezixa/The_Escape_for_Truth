using UnityEngine;

public class PlayerControls : MonoBehaviour
{

    public int Speed;
    public float inputX;
    public float inputY;
    private Rigidbody2D rb;


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


        Vector3 moveDir = Vector3.Normalize(new Vector2(inputX, inputY));
        rb.linearVelocity = moveDir * Speed;


    }
}
