using UnityEngine;

public class WanderAI : MonoBehaviour
{

    public float speed;
    public float rotationSpeed;
    public float changeDirectionDelay;
    float targetAngle;
    public Rigidbody2D rb;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        targetAngle = rb.rotation;
    }


    private void FixedUpdate()
    {
        UpdateTargetDirection();
        Rotate();
        Move();
    }


    private void UpdateTargetDirection()
    {
        RandomDirectionChange();
    }

    private void RandomDirectionChange()
    {
        changeDirectionDelay -= Time.deltaTime;
        if(changeDirectionDelay <= 0)
        {
            targetAngle += Random.Range(-90f,90f);
            changeDirectionDelay = Random.Range(1f, 5f);
        }
    }

    //rotate the NPC
    private void Rotate()
    {
        float newAngle = Mathf.MoveTowardsAngle(rb.rotation, targetAngle, rotationSpeed * Time.fixedDeltaTime);
        rb.MoveRotation(newAngle);
    }

    private void Move()
    {
        rb.linearVelocity = transform.right * speed;
    }
}
