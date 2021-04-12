using System.Collections;
using UnityEngine;

public class turnOnCollider : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(wait());
    }

    IEnumerator wait()
    {
        while (!AntColony.wasClick)
            yield return new WaitForSeconds(Time.deltaTime);

        yield return new WaitForSeconds(0.3f);
        GetComponent<Collider2D>().enabled = true;
    }
}
