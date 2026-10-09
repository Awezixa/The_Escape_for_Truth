using System;
using UnityEngine;
using UnityEngine.UIElements;

/// Pursue — chase a moving player by aiming ahead of them.
///
/// Goal: intercept the player, not just follow their current position.
/// Formula:
///   lookAheadTime = distanceToPlayer / maxSpeed
///   predictedPosition = playerPosition + playerVelocity * lookAheadTime
///   desiredVelocity = directionToPredictedPosition * maxSpeed
///

public class PursuePlayer : MonoBehaviour
{

    public float maxLookAheadTime = 2f;


    public float moveSpeed = 5f;
    Rigidbody2D rb;// guard rb
    Rigidbody2D targetRb; //player rb
    Transform target;
    bool inChase;
    public float sightRange = 5f;
    public float outOfSight= 10f;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //set the guard to target the player
        target = GameObject.Find("Player").transform;
        if (target)
        {
            targetRb = target.GetComponent<Rigidbody2D>();
        }
        rb = GetComponent<Rigidbody2D>();
    }


    //the chase mechanic
    private void FixedUpdate()
    {
        if (!target)
        {
            //if there is no target set linear vel to 0
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 myPos = rb.position;
        Vector2 targetPos = target.position;
        float Distance = Vector2.Distance(myPos, targetPos);

        //range for the chase
        if(!inChase && Distance <= sightRange)
        {
            inChase = true;
        }
        else if (inChase && Distance > outOfSight)
        {
            inChase = false;
        }

        if (!inChase)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 targetVelocity = Vector2.zero;
        if (targetRb != null)
        {
            targetVelocity = targetRb.linearVelocity;
        }

        float distance = Vector2.Distance(myPos, targetPos);
        float lookAheadTime = MathF.Min(distance/ moveSpeed, maxLookAheadTime);

        Vector2 predictedPos = targetPos + targetVelocity * lookAheadTime;
        Vector2 direction = (predictedPos - myPos).normalized;

        rb.linearVelocity = direction * moveSpeed;
        rb.rotation = Vector2.SignedAngle(Vector2.right, direction);
    }



    void OnDrawGizmosSelected()
    {
        if (!target) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(target.position, 0.2f);
    }
}
