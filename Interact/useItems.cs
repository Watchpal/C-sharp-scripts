using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class useItems : MonoBehaviour
{
    bool once = false;
    public string Tag;

    public Vector3 openHeight;
    public float duration = 1;
    Vector3 closePosition;

    void Start()
    {
        // Sets the first position of the door as it's closed position.
        closePosition = gameObject.transform.position;
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
        Vector3 startPosition = gameObject.transform.position;
        while (timeElapsed < duration)
        {
            gameObject.transform.position = Vector3.Lerp(startPosition, targetPosition, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        gameObject.transform.position = targetPosition;
    }
    // Start is called before the first frame update
    private void OnCollisionEnter(Collision collision)
    {
        if (!once && collision.gameObject.transform.GetChild(2).GetChild(0).gameObject.CompareTag(Tag))
        {
            OperateDoor();
            once = true;
        }
        
    }
}
