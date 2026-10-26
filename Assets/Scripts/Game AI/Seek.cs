using Unity.VisualScripting;
using UnityEngine;

public class Seek : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    private Rigidbody2D _rb;

    [SerializeField] private float moveSpeed = 3f;

    [SerializeField] private float detectionRadius = 5f;


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

        if(distance <= detectionRadius && distance > 0.1f)
        {
            _rb.linearVelocity = direction.normalized * moveSpeed;
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
    }



}
