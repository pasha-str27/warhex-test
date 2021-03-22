using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turnOnCollider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(wait());
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(1);
        GetComponent<Collider2D>().enabled = true;
    }
}
