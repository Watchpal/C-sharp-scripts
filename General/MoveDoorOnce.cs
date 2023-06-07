using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDoorOnce : MonoBehaviour
{
    [SerializeField]
    GameObject _object;
    private bool once = false;

    // Start is called before the first frame update
    public Vector3 openHeight;
    public float duration = 1;
    Vector3 closePosition;

    void Start()
    {
        // Sets the first position of the door as it's closed position.
        closePosition = _object.transform.position;
    }
    void OperateDoor()
    {
        StopAllCoroutines();
        Vector3 openPosition = closePosition + openHeight;
        StartCoroutine(Move(openPosition));
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
        if (!once)
        {
            OperateDoor();
            once = true;
        }
    }
}
