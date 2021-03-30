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
        yield return new WaitForSeconds(0.5f);
        GetComponent<Collider2D>().enabled = true;
    }
}
