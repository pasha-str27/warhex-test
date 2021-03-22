using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movePoint : MonoBehaviour
{
    public bool isPlayer;
    public Vector2 goal;
    public Vector2 beginLine;
    public Vector2 endLine;

    public float speed;

    public bool movingToEnemy;

    public static int EnemyNumber
    {
        get;set;
    }

    public static int PlayerNumber
    {
        get; set;
    }

    private void Start()
    {
        if (PlayerNumber == 3)
            PlayerNumber = -1;

        if (EnemyNumber == 0 || PlayerNumber == 0|| !isPlayer)
            movingToEnemy = true;
        if (!isPlayer)
            EnemyNumber++;
        else
            PlayerNumber++;
        if (!isPlayer || PlayerNumber <=3)
            beginLine = transform.position;
        //goal = endLine;
        //goal = new Vector2();
        //beginLine = transform.position;
        StartCoroutine(turnOnCollider());
    }

    public IEnumerator turnOnCollider()
    {
        yield return new WaitForSeconds(0.1f);

        GetComponent<Collider2D>().enabled = true;
        if (!movingToEnemy)
            movingToEnemy = true;
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, goal, Time.deltaTime * speed);
    }
}
