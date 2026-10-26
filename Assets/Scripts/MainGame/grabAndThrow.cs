using UnityEngine;

public class grabAndThrow : MonoBehaviour
{
    [SerializeField] private float rayDistance = 1.5f;
    [SerializeField] private float holdDistance = 0.8f;
    [SerializeField] private LayerMask grabMask;   // set in the Inspector: Throwable + the trolley's layer

    const string ThrowableTag = "Throwable";
    const string MoveableTag = "Movable";

    private GameObject grabbedObject;
    private Rigidbody2D grabbedRb;
    private bool isThrowable;
    private PlayerControls controls;
    private FixedJoint2D grabJoint;

    public float throwDistance = 4.0f;
    public float throwSpeed = 7.0f;
    public float momentum = 0.5f;

    private Rigidbody2D playerRb;

    private void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        controls = GetComponent<PlayerControls>();
    }

    void Update()
    {
        Vector2 dir = controls.facing;

        // held throwable follows the facing direction
        if (grabbedObject != null && isThrowable)
        {
            grabbedObject.transform.position = (Vector2)transform.position + dir * holdDistance;
        }

        Vector2 origin = (Vector2)transform.position + dir * 0.6f;
        RaycastHit2D hitInfo = Physics2D.CircleCast(origin, 0.4f, dir, rayDistance, grabMask);
        Debug.DrawRay(origin, dir * rayDistance, hitInfo.collider != null ? Color.green : Color.red);

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (grabbedObject == null && hitInfo.collider != null)
            {
                tryGrab(hitInfo.collider.gameObject, dir);
            }
            else if (grabbedObject != null && !isThrowable)
            {
                releaseObj();
            }
        }

        if (Input.GetKeyDown(KeyCode.T) && grabbedObject != null && isThrowable)
        {
            throwObj(dir);
        }
    }

    void tryGrab(GameObject obj, Vector2 dir)
    {
        bool throwable = obj.CompareTag(ThrowableTag);
        bool moveable = obj.CompareTag(MoveableTag);
        if (!throwable && !moveable) return;

        grabbedObject = obj;
        grabbedRb = obj.GetComponent<Rigidbody2D>();
        isThrowable = throwable;
        grabbedRb.linearVelocity = Vector2.zero;

        if (isThrowable)
        {
            grabbedRb.bodyType = RigidbodyType2D.Kinematic;
            obj.transform.position = (Vector2)transform.position + dir * holdDistance;
            obj.transform.SetParent(transform);
        }
        else
        {
            grabJoint = obj.AddComponent<FixedJoint2D>();
            grabJoint.connectedBody = playerRb;
            grabJoint.enableCollision = true;
        }
    }

    void releaseObj()
    {
        if (grabJoint != null)
        {
            Destroy(grabJoint);
            grabJoint = null;
        }
        grabbedRb.linearVelocity = Vector2.zero;
        ClearGrab();
    }

    void ClearGrab()
    {
        grabbedObject = null;
        grabbedRb = null;
    }

    void throwObj(Vector2 dir)
    {
        grabbedObject.transform.SetParent(null);
        grabbedRb.bodyType = RigidbodyType2D.Dynamic;

        Vector2 displacement = dir * throwDistance + playerRb.linearVelocity * momentum;
        grabbedRb.linearVelocity = displacement.normalized * throwSpeed;

        ClearGrab();
    }
}