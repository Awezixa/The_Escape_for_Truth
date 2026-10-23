using Unity.VisualScripting;
using UnityEngine;

public class grabAndThrow : MonoBehaviour
{

    [SerializeField] private float rayDistance = 1.5f;
    [SerializeField] private float holdDistance = 0.8f;   // where throwables are held

    const string ThrowableTag = "Throwable";
    const string MovabaleTag = "Moveable";

    //for grabbing the object
    private GameObject grabbedObject;// default set to null
    private Rigidbody2D grabbedRb;
    private bool isThrowable;
    private int layerIndex;
    private int layerMask;
    private PlayerControls controls;
 

    //physics for throwing the object
    //vairables for calculations
    public float throwDistance = 4.0f; // for when the player stands still
    public float throwSpeed = 7.0f;
    public float momentum = 0.5f;
    public float throwDuration = 1.0f;
    private float throwTimer;



    //calcualtion variables
    private Vector2 displacePos;
    private Vector2 throwStart;
    private Rigidbody2D playerRb;
    private Rigidbody2D thrownRb;//object currently in flight

    private void Start()
    {
        //throwing requirements
        layerIndex = LayerMask.NameToLayer("Objects");// start of game stores objects in obkect layer as ints for better performance
        playerRb = GetComponent<Rigidbody2D>();
        controls = GetComponent<PlayerControls>();

        if (layerIndex == -1)
        {
            Debug.LogError("Layer 'Objects' doesn't exist. Check the name in Project Settings > Tags and Layers.");
        }
        layerMask = 1 << layerIndex; // the ray will only hit this layer
    }

    void Update()
    {

        Vector2 dir = controls.facing;

        if (grabbedObject != null && isThrowable)
        {
            grabbedObject.transform.position = (Vector2)transform.position + dir * holdDistance;
        }

        Vector2 origin = (Vector2)transform.position + dir * 0.6f;


        RaycastHit2D hitInfo = Physics2D.CircleCast(origin, 0.4f, dir, rayDistance, layerMask);// creates a ray to determine length of ray
        Debug.DrawRay(origin, dir * rayDistance, hitInfo.collider != null ? Color.green : Color.red); // info about what ray hits its stored in hitInfo variable



        if (hitInfo.collider != null && hitInfo.collider.gameObject.layer == layerIndex)//checks that ray hitting is not nothing and that layer is correct, then we check inputs
        {
            //debug to see ray if hits or not. Green = hit, red = no hit
            Debug.DrawRay(origin, dir * rayDistance, hitInfo.collider != null ? Color.green : Color.red);


            //grab object
            if (Input.GetKeyDown(KeyCode.E))
            {
                if(grabbedObject == null && hitInfo.collider != null)
                {
                    tryGrab(hitInfo.collider.gameObject, dir);
                }
                else if (grabbedObject != null && !isThrowable)
                {
                    releaseObj();// leaves moveable object
                }

            }
            if (Input.GetKeyDown(KeyCode.T) && grabbedObject != null && isThrowable)
            {
                //throw object
                throwObj(dir);
                
                grabbedObject = null;//clear grabbed object variable


            }


        }

    }


    void FixedUpdate()
    {
        if (thrownRb == null) return;

        throwTimer -= Time.fixedDeltaTime;

        float travelled = Vector2.Distance(throwStart, thrownRb.position);
        float required = Vector2.Distance(throwStart, displacePos);

        // stop when the time is up or the target distance is reached
        if (throwTimer <= 0f || travelled >= required)
        {
            thrownRb.linearVelocity = Vector2.zero;
            thrownRb = null;
        }
    }


    void tryGrab(GameObject obj, Vector2 dir)
    {
        //checks the object type to see what to do
        bool throwable = obj.CompareTag("Throwable");
        bool movable = obj.CompareTag("Movable");
        if (!throwable && !movable) return; //not working then continue

        thrownRb = null; //clear any objects being thrown
        
        grabbedObject = obj;
        grabbedRb = obj.GetComponent<Rigidbody2D>();
        isThrowable = throwable;

        grabbedRb.linearVelocity = Vector2.zero;
        grabbedRb.bodyType = RigidbodyType2D.Kinematic;//changing rb to kinematic so can be thrown

        if (isThrowable)
        {
            obj.transform.position = (Vector2)transform.position + dir;
        }

        obj.transform.SetParent(transform);


    }

    void releaseObj()
    {
        //place object down
        grabbedRb.transform.SetParent(null);//set parent to null
        grabbedRb.bodyType = RigidbodyType2D.Dynamic;// change grabbed object back to dynamic
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
        thrownRb = grabbedRb;
        grabbedObject.transform.SetParent(null);// have to set parent to null since leaving the player parent
        thrownRb.bodyType = RigidbodyType2D.Dynamic;

        //displacement
        Vector2 displacement = dir * throwDistance + playerRb.linearVelocity * momentum;

        throwStart = thrownRb.position;
        displacePos = throwStart + displacement;
        throwTimer = throwDuration;
        thrownRb.linearVelocity = displacement.normalized * throwSpeed;

        ClearGrab();

    }



}
