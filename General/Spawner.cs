using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public GameObject spawnItem;

    public float frequency;

    public float initialSpeed;

    public bool created = true;

    public bool deleted = false;
    // Start is called before the first frame update

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!created)
        {
            Spawn();
            created = true;
            deleted = true;
        }
    }

    public void Spawn()
    {
        GameObject newSpawedOnject = Instantiate(spawnItem, transform.position, transform.rotation);
        newSpawedOnject.GetComponent<Rigidbody>().velocity = transform.forward * initialSpeed;
        //newSpawedOnject.transform.parent = transform;
    }
}
