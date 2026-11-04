using System;
using Unity.VisualScripting;
using UnityEngine;

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
    public float sightRange = 5f;
    public float outOfSight = 10f;
    public float moveSpeed = 5f;
    
    Rigidbody2D rb;// guard rb
    Rigidbody2D targetRb; //player rb
    Transform target;

    public bool inChase;
    



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //set the guard to target the player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player)
        {
            target = player.transform;
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
        float distance = Vector2.Distance(myPos, targetPos);


        // lost the player
        if (inChase && distance > outOfSight) inChase = false;

        if (!inChase)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 targetVelocity = targetRb != null ? targetRb.linearVelocity : Vector2.zero;


        float lookAheadTime = Mathf.Min(distance / moveSpeed, maxLookAheadTime);
        Vector2 predictedPos = targetPos + targetVelocity * lookAheadTime;
        Vector2 direction = (predictedPos - myPos).normalized;

        rb.linearVelocity = direction * moveSpeed;
        rb.rotation = Vector2.SignedAngle(Vector2.right, direction);
    }



    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, outOfSight);
    }
}
