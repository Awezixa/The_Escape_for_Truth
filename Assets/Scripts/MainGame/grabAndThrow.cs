using UnityEngine;

public class grabAndThrow : MonoBehaviour
{

    [SerializeField]
    private Transform grabPoint;


    [SerializeField]
    private Transform rayPoint;
    [SerializeField]
    private float rayDistance;

    //for grabbing the object
    private GameObject grabbedObject;// default set to null
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
        RaycastHit2D hitInfo = Physics2D.Raycast(rayPoint.position, transform.right, rayDistance, layerMask);// creates a ray to determine length of ray
                                                                                                             // info about what ray hits its stored in hitInfo variable

        if (hitInfo.collider != null && hitInfo.collider.gameObject.layer == layerIndex)//checks that ray hitting is not nothing and that layer is correct, then we check inputs
        {
            //debug to see ray if hits or not. Green = hit, red = no hit
            Debug.DrawRay(rayPoint.position, transform.right * rayDistance, hitInfo.collider != null ? Color.green : Color.red);


            //grab object
            if (Input.GetKeyDown(KeyCode.E) && grabbedObject == null)
            {
                grabbedObject = hitInfo.collider.gameObject; // stores grabbed object into variable
                grabbedObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic; // change grabbed object into kinematic
                grabbedObject.transform.position = grabPoint.position; // moving into position 
                grabbedObject.transform.SetParent(transform); //setting the player to be the parent object

                grabObject();
            }
            else if (Input.GetKeyDown(KeyCode.T) && grabbedObject != null)
            {
                //throw object
                thrownRb = grabbedObject.GetComponent<Rigidbody2D>();
                grabbedObject.transform.SetParent(null);// have to set parent to null since leaving the player parent
                thrownRb.bodyType = RigidbodyType2D.Dynamic;

                //displacement
                Vector2 displacement = dir * throwDistance + playerRb.linearVelocity * momentum;

                throwStart = thrownRb.position;
                displacePos = throwStart + displacement;
                throwTimer = throwDuration;
                thrownRb.linearVelocity = displacement.normalized * throwSpeed;


                //place object down
                //grabbedObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;// change grabbed object back to dynamic
                //grabbedObject.transform.SetParent(null);//set parent to null
                //grabbedObject = null;//clear grabbed object variable


            }


        }

        //if (playerNear && Input.GetKeyDown(KeyCode.E))
        //{
        //    grabObject();
        //}
        //if (Input.GetKeyDown(KeyCode.T) && grabbed)
        //{
        //    throwObject();
        //}
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

    void grabObject()
    {

        Debug.Log("Document Collected!");

    }

    //float throwObject()
    //{
    //    return displacePos = transform.player + ;
    //}








}
