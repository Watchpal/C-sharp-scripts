using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;
using UnityEngine.Rendering.VirtualTexturing;

public class TimeControlled : MonoBehaviour
{
    /*
        This script records the position and velocity of this object (unless time is being manipulated).
    */

    bool debugging = false;


    // State control
    public bool is_timeManipulated = false;
    bool        was_TimeManipulated = false;
    public bool grabbed = false;
    public bool chosen = true;

    public GameObject timeMaster;

    bool _isKinematic = false;

    // Data variables
    Rigidbody rigidBody;

    Vector3 previousPosition;
    public struct RecordedData
    {
        public Vector3 recordedPosisiton;
        public Quaternion recordedRotation;

        public Vector3 recordedVelocity;
        public Vector3 recordedAngularVelocity;

    }

    // Time Travel Variables
    public int rewindSpeedAmount = 0;
    
    int recordCount;
    int recordIndex;

    List<RecordedData> recordList = new List<RecordedData>();

    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<Rigidbody>())
        {
            rigidBody = GetComponent<Rigidbody>();

            if (rigidBody.isKinematic)
            {
                _isKinematic = true;
            }
        }
        else
        {
            if(debugging)Debug.LogError("TimeControlled: Object does not have a RigidBody");
        }

        timeMaster = GameObject.Find("Time_Master");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        /// IF: Time is being manipulated
        if (is_timeManipulated)
        {
            if (!grabbed && !chosen)
            {
                was_TimeManipulated = true;
                TimeUpdate();
            }
            /// PRIME: Reset 
            else if(grabbed || chosen)
            {
                if (previousPosition != transform.position)
                {
                    TimeRecord();
                }



                previousPosition = transform.position;
            }

                /* 
                 * if (!checkPosition)
                {
                    player.GetComponent<moveTogether>().enabled = true;
                    checkPosition = true;
                    
                }
                 pushVector = gameObject.transform.position - prePosition;

                 player.GetComponent<Rigidbody>().MovePosition((player.transform.position + pushVector));

                 prePosition = gameObject.transform.position;
               */
               
            

        }
        else
        {
            /// RESET: So that the newest entry is the current entry
            if (was_TimeManipulated)
            {
                recordCount = recordIndex;
                rewindSpeedAmount = 0;
                was_TimeManipulated = false;

                RemoveRecording(recordCount);
                //checkPosition = false;
                //player.GetComponent<moveTogether>().enabled = false;
                //rb.useGravity = true;

            }

            if (previousPosition != transform.position)
            {
                TimeRecord();
            }
            


            previousPosition = transform.position;

        }
    }
    ///









    /// Travel in Time
    public void TimeUpdate()
    {
       
        /// IF: Rewind speed isn't zero
        if (rewindSpeedAmount != 0)
        {
        // Step through recording
        recordIndex -= rewindSpeedAmount;

        if(debugging)Debug.Log("RecordIndex: " + recordIndex + " recordList.Count: " + recordList.Count);

            // CLAMP: RecordIndex so it does not go out of bounds  
            recordIndex = Mathf.Clamp(recordIndex, 1, recordList.Count - 2);
        }



        /// SET: Position to past position
        RecordedData data = recordList[recordIndex];
        gameObject.transform.position = data.recordedPosisiton;
        gameObject.transform.rotation = data.recordedRotation;

        // SET: Velocity to past velocity
        if (gameObject.GetComponent<Rigidbody>())
        {
            rigidBody.velocity = data.recordedVelocity;
            rigidBody.angularVelocity = data.recordedAngularVelocity;


        }

        if (debugging) Debug.Log("-- NOW: " + recordList.Count);

        /// IF: TimeMaster has stopped time travel
        if (timeMaster != null)
        { 
            if(timeMaster.GetComponent<Time_Control>().timeState == Time_Control.timeWinding.none)
            {
                is_timeManipulated = false;
            }

        }

    }
    ///

    /// Record points in Time
    void TimeRecord()
    {
        // Get component
        RecordedData data = new RecordedData();

        /// Update Component

        //  CHECK FOR RIGIDBODY                    
        if (gameObject.GetComponent<Rigidbody>())
        {
            // Record Velocity 
            data.recordedVelocity = rigidBody.velocity;
            data.recordedAngularVelocity = rigidBody.angularVelocity;

            // I: Originally "isKinematic", don't turn it off.
            if (!_isKinematic)
            {
                rigidBody.isKinematic = false;
            }

        }

        // Record position
        data.recordedPosisiton = transform.position;
        data.recordedRotation = transform.rotation;

        // SET: Data in array
        recordList.Add( data );  //[recordCount] = data;

        // Increment
        recordCount++;
        recordIndex = recordCount;

        if (debugging) Debug.Log("recordList.Count: "+ recordList.Count);


    }
    ///

    /// / REMOVE: recordings
    void RemoveRecording(int downTo)
    {
        if (debugging)
        {
            Debug.Log("REMOVING:: recordList.Count   : " + recordList.Count +" Remove Down to: " + downTo);
            Debug.Log("- - - -");
        }

        int _i;
        for(_i = recordList.Count; _i > downTo; _i--)
        {
            recordList.RemoveAt( _i - 1);  //recordList.Count - 1);
        }

        if (debugging) Debug.Log("recordCount is Now: " + recordList.Count);
    }
    ///

}
