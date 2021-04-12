using System.Collections;
using UnityEngine;

public class movePoint : MonoBehaviour
{
    public bool isPlayer;
    public Vector2 goal;
    public Vector2 beginLine;
    public Vector2 endLine;

    public static bool letMovePoint;

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

    int maxPlayerCount = 3;

    private void Start()
    {
        letMovePoint = true;
        if (PlayerNumber == maxPlayerCount)
            PlayerNumber = -1;

        if (EnemyNumber == 0 || PlayerNumber == 0|| !isPlayer)
            movingToEnemy = true;
        if (!isPlayer)
            EnemyNumber++;
        else
            PlayerNumber++;

        if (!isPlayer || PlayerNumber <= maxPlayerCount)
            beginLine = transform.position;

        StartCoroutine(turnOnCollider());
    }

    public IEnumerator turnOnCollider()
    {
        yield return new WaitForSeconds(0.05f);

        GetComponent<Collider2D>().enabled = true;
        if (!movingToEnemy)
            movingToEnemy = true;
    }

    //move point to goal
    void Update()
    {
        if(letMovePoint)
            transform.position = Vector2.MoveTowards(transform.position, goal, Time.deltaTime * speed);
    }
}
