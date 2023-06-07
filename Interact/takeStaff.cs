using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class takeStaff : MonoBehaviour
{
    GameObject objectInHand;
    Transform Hand;

    private void Awake()
    {
        Hand = this.gameObject.transform.GetChild(2);
    }
    // Start is called before the first frame update

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wand") && !gameObject.GetComponent<Player3D_Grab>().grabLight)
        {
            objectInHand = collision.gameObject;
            Destroy(objectInHand.GetComponent<Rigidbody>());
            objectInHand.transform.parent = Hand;
            objectInHand.transform.localPosition = objectInHand.GetComponent<Wand>().pickPosition;
            objectInHand.transform.localEulerAngles = objectInHand.GetComponent<Wand>().pickRotation;
            if (objectInHand.GetComponent<TimeControlled>() != null)
            {
                objectInHand.GetComponent<TimeControlled>().grabbed = true;
            }
            gameObject.GetComponent<Player3D_Grab>().grabbedObject = objectInHand;
            gameObject.GetComponent<Player3D_Grab>().LightObject = objectInHand;
            gameObject.GetComponent<Player3D_Grab>().grabLight = true;
            gameObject.GetComponent<Player3D_Grab>().grabType = objectInHand.GetComponent<InteractableClass>().interactType;
        }
    }
  /*  private void OnTriggerEnter(Collider other)
    {
        Debug.Log("testWand");
        if (other.gameObject.CompareTag("Wand"))
        {
            objectInHand = other.gameObject;
            objectInHand.transform.parent = Hand;
            objectInHand.transform.localPosition = myWand.pickPosition;
            objectInHand.transform.localEulerAngles = myWand.pickRotation;
        }
    }
       */
}
