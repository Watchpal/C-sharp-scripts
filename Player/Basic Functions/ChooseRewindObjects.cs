using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChooseRewindObjects : MonoBehaviour
{

    public float reachRewind = 3f;
    public LayerMask interactLayer1;
    GameObject takeObject;
    // Start is called before the first frame update

    public void Choose(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Choose");
            takeObject = DetectGrabObject();
            Debug.Log(takeObject);
            if (takeObject != null && !takeObject.GetComponent<TimeControlled>().chosen)
            {
                takeObject.GetComponent<TimeControlled>().chosen = true;
                takeObject.GetComponent<Renderer>().material.color = Color.white;
                Debug.Log("Choose item");
            }
            else if(takeObject != null && takeObject.GetComponent<TimeControlled>().chosen)
            {
                takeObject.GetComponent<TimeControlled>().chosen = false;
                
                takeObject.GetComponent<Renderer>().material.color = Color.red;
            }
        }
        
    }

    GameObject DetectGrabObject()
    {
        // Check if we have a grabbable object in front of us


        //bool grabCheck = Physics.BoxCast(transform.position,grabReach,transform.forward);//(transform.position, Vector2.one * grabReach, 0f, Vector2.zero, 1f, interactLayer);
        Debug.Log("Cast Ray123123");


        //new Vector3(transform.position.x, transform.position.y, transform.position.z + 2f)
        Vector3 rayVec = transform.position;

        //new Vector3(transform.position.x, transform.position.y, transform.position.z + 2f)
        Ray theRay = new Ray(rayVec, transform.TransformDirection(Vector3.forward * reachRewind));

        if (Physics.Raycast(theRay, out RaycastHit hit, reachRewind, interactLayer1))
        {
            
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
        Gizmos.DrawRay(rayVec, transform.TransformDirection(Vector3.forward * reachRewind));//DrawWireCube(transform.position, grabReach);
    }
}
