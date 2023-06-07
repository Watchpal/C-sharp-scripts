using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPressure_moveObject : MonoBehaviour
{
    [SerializeField]
    GameObject _object;

    public Vector3 objectDestination;
    private bool switcher = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!switcher)
        {
            _object.transform.position += objectDestination;
            switcher = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (switcher)
        {
            _object.transform.position -= objectDestination;
            switcher = false;
        }


    }

}
