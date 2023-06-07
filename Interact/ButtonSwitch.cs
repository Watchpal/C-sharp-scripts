using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSwitch : MonoBehaviour
{
    [SerializeField]
    GameObject _object;

    public Vector3 objectDestination;
    private bool switcher = false;
    private bool iNeedThisForPlayer = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!switcher && !iNeedThisForPlayer)
        {
            _object.transform.position += objectDestination;
            switcher = true;
            iNeedThisForPlayer = true;
        }

        if (switcher && !iNeedThisForPlayer)
        {
            _object.transform.position -= objectDestination;
            switcher = false;
            iNeedThisForPlayer = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (iNeedThisForPlayer)
        {
            iNeedThisForPlayer = false;
            
        }
        

    }
}
