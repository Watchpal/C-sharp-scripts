using AC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class Player3D_Grab : MonoBehaviour
{
    // Carrier is the character carring the grabbed object

    // Grabbed object is the object being grabbed

    

    [Header("Carrier")]
    public GameObject carrierHand;
    Transform grabPoint;
    public LayerMask interactLayer;
    public Vector3 handOffset;
    public float grabReach = 3f;
    public bool grabLight = false;

    public float ForceThrow = 5.0f;
    public float _drag = 1f;

    Transform Hand;
    //public InputAction playerAction;


    private Vector3 grabDragDifference;
    private Vector3 grabDragDifferenceHeavy;

    public bool isGrabbing;
    public GameObject grabbedObject;
    public GameObject LightObject;
    public string grabType;

    float carrierSpeed;

    bool debugging = false;

    private void Awake()
    {
        Hand = this.gameObject.transform.GetChild(2);
    }

    // Start is called before the first frame update
    void Start()
    {

        isGrabbing = gameObject.GetComponent<Player_Stats>().isGrabbing;
        carrierHand = gameObject;
        grabPoint = carrierHand.transform;
    }


    public void Grab(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (context.interaction is HoldInteraction)
            {
                if (grabLight && !gameObject.GetComponent<Player_Stats>().isGrabbing)
                {
                    //gameObject.GetComponent<Player_Stats>().isGrabbing = false;
                    Invoke("WaitToThrow", 0.25f);
                    // Set altered variables to their origin
                    //grabbedObject.GetComponent<TimeControlled>().enabled = true;

                    LightObject.transform.parent = null;
                    Rigidbody rbItem = LightObject.AddComponent<Rigidbody>();
                    rbItem.AddForce(transform.forward * ForceThrow, ForceMode.Impulse);
                    rbItem.drag = _drag;
                    grabbedObject = null;
                    LightObject = null;
                    
                }
            }
            else
            {
                GrabDropObject();
            }
        }
        
    }
    // Update is called once per frame

    // Update is called once per physics frame 
    private void FixedUpdate()
    {
        if (grabbedObject != null)
        {
            // Move grabbed Object
            switch (grabType)
            {
                case ("Grab Light"):
                    // Hold Object
                    //grabbedObject.GetComponent<Rigidbody>().MovePosition(carrierHand.transform.position + handOffset);//transform.position = carrierHand.transform.position + handOffset;

                    break;
                case ("Grab Heavy"):

                    // Get Push Vector
                    Vector3 pushVector = gameObject.transform.position - grabDragDifference;

                    //Debug.Log("Push Vector :: " + (grabbedObject.transform.position + pushVector));

                    // Push Object
                    grabbedObject.GetComponent<Rigidbody>().MovePosition((grabbedObject.transform.position + pushVector)); //* 1.2f ); 

                    // Update vector for next "Get Push Vector"
                    grabDragDifference = gameObject.transform.position;

                    break;

                case ("Grab Too Heavy"):
                    Vector3 pushVector2 = grabbedObject.transform.position - grabDragDifferenceHeavy;

                    //Debug.Log("Push Vector :: " + (grabbedObject.transform.position + pushVector));

                    // Push Object
                    gameObject.GetComponent<Rigidbody>().MovePosition((gameObject.transform.position + pushVector2)); //* 1.2f ); 

                    // Update vector for next "Get Push Vector"
                    grabDragDifferenceHeavy = grabbedObject.transform.position;
                    break;
            }
        }
    }

    public void WaitToThrow()
    {
        grabLight = false;
    }
    public void WaitToGrab()
    {
        gameObject.GetComponent<Player_Stats>().isGrabbing = false;
    }

    public bool GrabDropObject()
    {
        // If we have something in our hands
        // Let go
        if (gameObject.GetComponent<Player_Stats>().isGrabbing)
        {
            //gameObject.GetComponent<Player_Stats>().isGrabbing = false;
            
            // Set altered variables to their origin
            //grabbedObject.GetComponent<TimeControlled>().enabled = true;
            if(grabbedObject.GetComponent<TimeControlled>() != null)
            {
                grabbedObject.GetComponent<TimeControlled>().grabbed = false;
            }
            
            switch (grabType)
            {
                case ("Grab Heavy"):
                    Invoke("WaitToGrab", 0.25f);
                    // reset Carrier Speed
                    gameObject.GetComponent<PlayerMove3D>().moveSpeed = carrierSpeed;
                    //gameObject.GetComponent<PlayerMove3D>().maxSpeed = carrierMaxSpeed;

                    // reset All variables
                    grabbedObject = null;
                    carrierSpeed = 0f;  
                    break;

                case ("Grab Too Heavy"):
                    Invoke("WaitToGrab", 0.25f);
                    gameObject.GetComponent<PlayerMove3D>().moveSpeed = carrierSpeed;
                    gameObject.GetComponent<Rigidbody>().useGravity = true;
                    gameObject.GetComponent<Player3D_Jump>().grabbed = false;
                    grabbedObject = null;
                    carrierSpeed = 0f;
                    break;
            }
            return (isGrabbing);

        }
        // If we are empty handed
        // Pick up
        else
        {
            // // // Checking for objects to grab
            if(debugging)Debug.Log("Look for Item to grab");
            grabbedObject = DetectGrabObject();

            if (grabbedObject != null)
            {
                //grabbedObject.GetComponent<TimeControlled>().enabled = false;
                if (grabbedObject.GetComponent<TimeControlled>() != null)
                {
                    grabbedObject.GetComponent<TimeControlled>().grabbed = true;
                }
                
                // Tell Stats that we are grabbing
                if (debugging)Debug.Log("Grabbed Obejct = " + grabbedObject.name);

                grabDragDifference = gameObject.transform.position;
                grabDragDifferenceHeavy = grabbedObject.transform.position;
                
                grabType = grabbedObject.GetComponent<InteractableClass>().interactType;

                // Identify what type of object
                switch (grabType)
                {
                    case ("Grab Light"):
                        // Set HandOffset
                        //handOffset = new Vector3(1.3f,1f,0f);

                        // Remove Gravity from object
                        Destroy(grabbedObject.GetComponent<Rigidbody>());
                        grabbedObject.transform.parent = Hand;
                        grabbedObject.transform.localPosition = grabbedObject.GetComponent<Wand>().pickPosition;
                        grabbedObject.transform.localEulerAngles = grabbedObject.GetComponent<Wand>().pickRotation;
                        grabLight = true;
                        LightObject = grabbedObject;

                        break;

                    case ("Grab Heavy"):
                        // Set HandOffset
                        //handOffset = new Vector3(1.3f, 0f, 0f);
                        gameObject.GetComponent<Player_Stats>().isGrabbing = true;
                        // Store Player speed
                        carrierSpeed = gameObject.GetComponent<PlayerMove3D>().moveSpeed;
                        //carrierMaxSpeed = gameObject.GetComponent<PlayerMove3D>().maxSpeed;

                        // Change Player speed
                        gameObject.GetComponent<PlayerMove3D>().moveSpeed = carrierSpeed / 4;
                        //gameObject.GetComponent<PlayerMoveKeys>().maxSpeed = carrierMaxSpeed / 4;
                        // 

                        handOffset = grabbedObject.transform.position - carrierHand.transform.position;


                        break;

                    case ("Grab Too Heavy"):
                        gameObject.GetComponent<Player_Stats>().isGrabbing = true;
                        carrierSpeed = gameObject.GetComponent<PlayerMove3D>().moveSpeed;

                        gameObject.GetComponent<PlayerMove3D>().moveSpeed = 0;
                        gameObject.GetComponent<Rigidbody>().useGravity = false; 

                        handOffset = grabbedObject.transform.position - carrierHand.transform.position;

                        grabbedObject.GetComponent<TimeControlled>().grabbed = false;

                        gameObject.GetComponent<Player3D_Jump>().grabbed = true;

                        break;
                }

            }

            return (gameObject.GetComponent<Player_Stats>().isGrabbing);
        }
    }

    void flipGrabbedObject()
    {
        // Flip position if Carrier is flipped
    }

    GameObject DetectGrabObject()
    {
        // Check if we have a grabbable object in front of us


        //bool grabCheck = Physics.BoxCast(transform.position,grabReach,transform.forward);//(transform.position, Vector2.one * grabReach, 0f, Vector2.zero, 1f, interactLayer);
        //Debug.Log("Cast Ray");

        Vector3 rayVec = transform.position;

        //new Vector3(transform.position.x, transform.position.y, transform.position.z + 2f)
        Ray theRay = new Ray( rayVec , transform.TransformDirection(Vector3.forward * grabReach));

        if (Physics.Raycast(theRay, out RaycastHit hit, grabReach, interactLayer))
        {
            /*if (hit.collider.tag == "interact")//gameObject.GetComponent<InteractableClass>())
            {
                Debug.Log("FOUND THE CUBE");
                return (hit.transform.gameObject);
            }
            else
            {
                Debug.Log("Did not find Cube");
                return (null);
                //return (hit.transform.gameObject);
            }*/
            if (debugging)Debug.Log("FOUND THE CUBE");
            return (hit.transform.gameObject);

        }
        else
        {

            return (null);
        }

    }

    private void OnDrawGizmos()
    {
        Vector3 rayVec = transform.position;
        Gizmos.color = Color.red;
        Gizmos.DrawRay( rayVec , transform.TransformDirection(Vector3.forward * grabReach));//DrawWireCube(transform.position, grabReach);
    }
}

