using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float movespeed = 0.01f;
    Vector3 pointA;
    Vector3 pointB;
    public int distance = 1;
    TimeControlled checkRewind;
    bool rewind;
    float timer;
    float time;
    // Start is called before the first frame update
    void Start()
    {
        checkRewind = GameObject.FindGameObjectWithTag("Platform").GetComponent<TimeControlled>();
        
        pointA = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        pointB = new Vector3(transform.position.x, transform.position.y + distance, transform.position.z);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rewind = checkRewind.is_timeManipulated;
        if (!rewind)
        {
            timer += Time.deltaTime;  
            time = Mathf.PingPong(timer * movespeed, 1);
            transform.position = Vector3.Lerp(pointA, pointB, time);
            //transform.position = transform.position + new Vector3(0, -movespeed, 0);

            //Debug.Log(transform.position);
        }
        else
        {
            timer -= Time.deltaTime;
            time = Mathf.PingPong(timer * movespeed, 1);
            transform.position = Vector3.Lerp(pointA, pointB, time);
        }
        //Debug.Log(transform.position);
    }
}
