using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turnGravityOn : MonoBehaviour
{

    private Rigidbody rb;
    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        rb.useGravity = true;
    }
}
