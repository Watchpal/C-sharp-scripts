using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDoorSwitch : MonoBehaviour
{
    [SerializeField]
    GameObject _object;
    private bool iNeedThisForPlayer = false;

    // Start is called before the first frame update
    public Vector3 openHeight;
    public float duration = 1;
    bool doorOpen = false;
    Vector3 closePosition;

    void Start()
    {
        // Sets the first position of the door as it's closed position.
        closePosition = _object.transform.position;
    }
    void OperateDoor()
    {
        StopAllCoroutines();
        if (!doorOpen)
        {
            Vector3 openPosition = closePosition + openHeight;
            StartCoroutine(Move(openPosition));
        }
        else
        {
            StartCoroutine(Move(closePosition));
        }
        doorOpen = !doorOpen;
    }
    IEnumerator Move(Vector3 targetPosition)
    {
        float timeElapsed = 0;
        Vector3 startPosition = _object.transform.position;
        while (timeElapsed < duration)
        {
            _object.transform.position = Vector3.Lerp(startPosition, targetPosition, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        _object.transform.position = targetPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!iNeedThisForPlayer)
        {
            OperateDoor();
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
