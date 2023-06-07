using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despawner : MonoBehaviour
{

    public Spawner spwn;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Cylinder"))
        {
            spwn.created = false;
        }
        Destroy(other.gameObject);

    }
}
