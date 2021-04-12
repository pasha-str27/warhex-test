using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnOnePoint : MonoBehaviour
{
    public GameObject enemy;
    public Vector2 goal;

    void Start()
    {
        GameObject point = Instantiate(enemy, transform.position, Quaternion.identity);
        var movePoint_ = point.GetComponent<movePoint>();
        movePoint_.endLine = goal;
        movePoint_.goal = goal;
        movePoint_.speed = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
