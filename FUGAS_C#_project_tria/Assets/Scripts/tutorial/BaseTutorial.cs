using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTutorial : MonoBehaviour
{
    static PlayerManager playerManager_;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if base was conquered early then set 'zeroes' values for enemy

        movePoint movePoint_ = collision.GetComponent<movePoint>();
        Vector3 pos = transform.position;

        if (movePoint_.movingToEnemy)
            movePoint_.goal = movePoint_.beginLine;
        else
            movePoint_.goal = movePoint_.endLine;

        movePoint_.movingToEnemy = !movePoint_.movingToEnemy;
    }
}
