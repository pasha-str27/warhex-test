using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Base : MonoBehaviour
{
    public int playerScore;
    public int enemyScore;

    public GameObject playerPoint;
    public GameObject enemyPoint;
    static Text allSpheres;

    public static int CurrentMaxIndexAI;
    int indexForAI;

    private void Start()
    {
        indexForAI = 0;
        allSpheres = GameObject.FindGameObjectWithTag("allSpheres").GetComponent<Text>();
    }

    bool findLineInPlayerAndAI(LineRenderer line)
    {
        foreach (var base_ in PlayerManager.conqueredBases)
            if(new Vector2(base_.transform.position.x, base_.transform.position.y).Equals(line.GetPosition(0))
                || new Vector2(base_.transform.position.x, base_.transform.position.y).Equals(line.GetPosition(1)))
            return false;

        foreach (var base_ in AIManager.conqueredBases)
            if (new Vector2(base_.transform.position.x, base_.transform.position.y).Equals(line.GetPosition(0))
                || new Vector2(base_.transform.position.x, base_.transform.position.y).Equals(line.GetPosition(1)))
                return false;
        return true ;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (enemyScore == 10 && collision.GetComponent<movePoint>().isPlayer)
        {
            playerScore=1;
            --enemyScore;
        }
        else
        {
            if (playerScore == 10 && !collision.GetComponent<movePoint>().isPlayer)
            {
                --playerScore;
                enemyScore=1;
            }
            else
            {
                if (enemyScore <= 9 && playerScore <= 9)
                {
                    if (collision.GetComponent<movePoint>().isPlayer)
                    {
                        ++playerScore;
                        if (enemyScore > 0)
                            --enemyScore;
                    }
                    else
                    {
                        if(playerScore>0)
                            --playerScore;
                        ++enemyScore;
                    }

                    if (gameObject.tag == "Point")
                    {
                        //if (playerScore >= 0)
                        gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().material.SetFloat("_Arc1", 360 - playerScore * 36);
                        //if (enemyScore >= 0)
                        gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>().material.SetFloat("_Arc2", 360 - enemyScore * 36);

                        if (playerScore == 10)
                        {
                            AntColony colorChanger = GameObject.FindGameObjectWithTag("antColony").GetComponent<AntColony>();
                            foreach (var line in colorChanger.LineRenderersList)
                                if (new Vector2(line.GetPosition(0).x, line.GetPosition(0).y).Equals(transform.position) || new Vector2(line.GetPosition(1).x, line.GetPosition(1).y).Equals(transform.position))
                                {
                                    line.startColor = Color.gray;
                                    line.endColor = Color.gray;
                                }

                            if(indexForAI>0)
                            {
                                for (int i = AIManager.conqueredBases.Count - 1; i >= indexForAI; --i)
                                {
                                    AIManager.conqueredBases[i].GetComponent<SpriteRenderer>().color = Color.white;
                                    AIManager.conqueredBases[i].GetComponent<Base>().enemyScore = 0;
                                    AIManager.conqueredBases[i].transform.GetChild(1).GetComponent<SpriteRenderer>().material.SetFloat("_Arc2", 360);
                                    AIManager.conqueredBases.RemoveAt(i);
                                    Destroy(AIManager.spheres[i-1]);
                                    AIManager.spheres.RemoveAt(i-1);
                                }

                                //зробити сірими лінії між незахопленими базами

                                foreach (var line in colorChanger.LineRenderersList)
                                    if(findLineInPlayerAndAI(line))
                                    {
                                        line.startColor = Color.gray;
                                        line.endColor = Color.gray;
                                    }

                                CurrentMaxIndexAI = indexForAI;
                            }


                            gameObject.GetComponent<SpriteRenderer>().color = Color.blue;



                            if (PlayerManager.conqueredBases.IndexOf(gameObject) == -1)
                                PlayerManager.conqueredBases.Add((gameObject));



                            for (int i = 0; i < PlayerManager.conqueredBases.Count ; ++i)
                                colorChanger.ChangeLineColor(gameObject.transform.position, PlayerManager.conqueredBases[i].transform.position, Color.blue);


                            indexForAI = 0;
                        }
                        if (enemyScore == 10)
                        {
                            ++CurrentMaxIndexAI;
                            indexForAI = CurrentMaxIndexAI;
                            AntColony colorChanger = GameObject.FindGameObjectWithTag("antColony").GetComponent<AntColony>();

                            //для всіх ліній сірий колір

                            foreach(var line in colorChanger.LineRenderersList)
                                if(new Vector2(line.GetPosition(0).x, line.GetPosition(0).y).Equals(transform.position)|| new Vector2(line.GetPosition(1).x, line.GetPosition(1).y).Equals(transform.position))
                                {
                                    line.startColor = Color.gray;
                                    line.endColor = Color.gray;
                                }

                            PlayerManager.conqueredBases.Remove(gameObject);

                            //видалення баз і ліній з захопленої області

                            colorChanger.ChangeLineColor(collision.GetComponent<movePoint>().beginLine, collision.GetComponent<movePoint>().endLine, Color.red);
                            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                            
                            if ( !transform.position.Equals(AIManager.finishBase))
                            {
                                GameObject point = Instantiate(enemyPoint, transform.position, Quaternion.identity);
                                point.GetComponent<movePoint>().endLine = GameObject.FindGameObjectWithTag("antColony").GetComponent<AntColony>().ColonyWay(movePoint.EnemyNumber).Origin;
                                point.GetComponent<movePoint>().goal = point.GetComponent<movePoint>().endLine;
                                AIManager.conqueredBases.Add(gameObject);
                                AIManager.spheres.Add(point);
                                AIManager.playerLines.Add(new Assets.TestScripts.triangulation.Line(point.GetComponent<movePoint>().beginLine, transform.position));
                            }
                        }     
                    }
                }
            }
        }
        

        if (collision.GetComponent<movePoint>().movingToEnemy)
            collision.GetComponent<movePoint>().goal = collision.GetComponent<movePoint>().beginLine;
        else
            collision.GetComponent<movePoint>().goal = collision.GetComponent<movePoint>().endLine;

        collision.GetComponent<movePoint>().movingToEnemy = !collision.GetComponent<movePoint>().movingToEnemy;

    }
}
