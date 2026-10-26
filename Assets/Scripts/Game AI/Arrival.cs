using Unity.VisualScripting;
using UnityEngine;

public class Arrival : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    private Rigidbody2D _rb;

    [SerializeField] private float moveSpeed = 3f;

    [SerializeField] private float detectionRadius = 3f;
    [SerializeField] private float slowRadius = 1.5f;


    private void Awake()
    {
        //_playerTransform = GameObject.FindGameObjectsWithTag("Player").transform;
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Vector2 direction = (Vector2)_playerTransform.position - _rb.position;
        
        float distance = direction.magnitude;

        float speed = moveSpeed * Mathf.InverseLerp(0.1f, slowRadius, distance);

        if(distance <= detectionRadius && distance > 0.1f)
        {
            _rb.linearVelocity = direction.normalized * speed;
        }
        else
        {
            _rb.linearVelocity = Vector3.zero;
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, slowRadius);
    }



}
