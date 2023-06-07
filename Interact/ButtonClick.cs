using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClick : MonoBehaviour
{
    [SerializeField]
    GameObject _object;

    public Vector3 objectDestination;
    private bool once = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!once)
        {
            _object.transform.position += objectDestination;
            once = true;
        }
    }
}
