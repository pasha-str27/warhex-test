using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Base : MonoBehaviour
{
    public int playerScore;
    public int enemyScore;

    public Color playerColor;
    public Color enemyColor;

    public GameObject playerPoint;
    public GameObject enemyPoint;
    //static Text allSpheres;

    AIManager AIManager_;
    PlayerManager playerManager_;

    public GameObject conqueredBaseEffectByPlayer;
    public GameObject conqueredBaseEffectByComputer;

    public static int CurrentMaxIndexAI;
    int indexForAI;

    public bool isEnamyBase;

    static GameObject endGame;
    static GameObject winFrame;
    static GameObject loseFrame;
    static GameObject pauseButton;
    static GameObject menuButton;
    static GameObject restartLevelButton;
    static GameObject nextLevelButton;

    private void Start()
    {
        if(endGame==null)
        {
            endGame=GameObject.FindGameObjectWithTag("endGame");
            winFrame= GameObject.FindGameObjectWithTag("winFrame");
            pauseButton=GameObject.FindGameObjectWithTag("pauseButton");
            restartLevelButton=GameObject.FindGameObjectWithTag("restartLevelButton");
            nextLevelButton=GameObject.FindGameObjectWithTag("nextLevelButton");
            loseFrame = GameObject.FindGameObjectWithTag("loseFrame");
            menuButton = GameObject.FindGameObjectWithTag("menuButton");
            //GameObject.FindGameObjectWithTag("restartLevelButton");
            endGame.SetActive(false);
            winFrame.SetActive(false);
            loseFrame.SetActive(false);
            pauseButton.SetActive(true);
            menuButton.SetActive(true);
            restartLevelButton.SetActive(true);
            //restartLevelButton.SetActive(true);
            nextLevelButton.SetActive(false);
        }

        playerManager_ = GameObject.FindGameObjectWithTag("playerController").GetComponent<PlayerManager>();
        AIManager_ = GameObject.FindGameObjectWithTag("AIController").GetComponent<AIManager>();
        indexForAI = 0;
        isEnamyBase = false;
        //allSpheres = GameObject.FindGameObjectWithTag("allSpheres").GetComponent<Text>();
    }

    bool findLineInPlayerAndAI(LineRenderer line)
    {
        foreach (var base_ in playerManager_.conqueredBases)
            if(new Vector2(base_.transform.position.x, base_.transform.position.y).Equals(line.GetPosition(0))
                || new Vector2(base_.transform.position.x, base_.transform.position.y).Equals(line.GetPosition(1)))
            return false;

        foreach (var base_ in AIManager_.conqueredBases)
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

                        if (playerScore == 10 && enemyScore == 0)
                        {
                            Destroy(Instantiate(conqueredBaseEffectByPlayer), 1);
                            isEnamyBase = false;
                            AntColony colorChanger = GameObject.FindGameObjectWithTag("antColony").GetComponent<AntColony>();
                            foreach (var line in colorChanger.LineRenderersList)
                                if (new Vector2(line.GetPosition(0).x, line.GetPosition(0).y).Equals(transform.position) || new Vector2(line.GetPosition(1).x, line.GetPosition(1).y).Equals(transform.position))
                                {
                                    line.startColor = Color.gray;
                                    line.endColor = Color.gray;
                                }

                            if(indexForAI > 0)
                            {
                                print(AIManager_.conqueredBases.Count);
                                print(indexForAI);
                                for (int i = AIManager_.conqueredBases.Count - 1; i >= indexForAI; --i)
                                {
                                    AIManager_.conqueredBases[i].GetComponent<SpriteRenderer>().color = Color.white;
                                    AIManager_.conqueredBases[i].GetComponent<Base>().enemyScore = 0;
                                    AIManager_.conqueredBases[i].GetComponent<Base>().isEnamyBase = false;
                                    AIManager_.conqueredBases[i].transform.GetChild(1).GetComponent<SpriteRenderer>().material.SetFloat("_Arc2", 360);
                                    AIManager_.conqueredBases.RemoveAt(i);
                                    Destroy(AIManager_.spheres[i-1]);
                                    movePoint.EnemyNumber--;
                                    AIManager_.spheres.RemoveAt(i-1);
                                }
                                print(AIManager_.conqueredBases.Count);

                                //зробити сірими лінії між незахопленими базами

                                foreach (var line in colorChanger.LineRenderersList)
                                    if(findLineInPlayerAndAI(line))
                                    {
                                        line.startColor = Color.gray;
                                        line.endColor = Color.gray;
                                    }

                                CurrentMaxIndexAI = indexForAI;
                            }


                            gameObject.GetComponent<SpriteRenderer>().color = playerColor;



                            if (playerManager_.conqueredBases.IndexOf(gameObject) == -1)
                                playerManager_.conqueredBases.Add((gameObject));



                            for (int i = 0; i < playerManager_.conqueredBases.Count ; ++i)
                                colorChanger.ChangeLineColor(gameObject.transform.position, playerManager_.conqueredBases[i].transform.position, playerColor);

                            indexForAI = 0;
                            if (transform.position.Equals(playerManager_.finishBase))
                            {
                                StartCoroutine(youWin());
                            }
                        }
                        else
                        {
                            if (enemyScore == 10 && playerScore == 0)
                            {
                                AntColony colorChanger = GameObject.FindGameObjectWithTag("antColony").GetComponent<AntColony>();
                                Destroy(Instantiate(conqueredBaseEffectByComputer), 1);
                                //для всіх ліній сірий колір

                                foreach (var line in colorChanger.LineRenderersList)
                                    if (new Vector2(line.GetPosition(0).x, line.GetPosition(0).y).Equals(transform.position) || new Vector2(line.GetPosition(1).x, line.GetPosition(1).y).Equals(transform.position))
                                    {
                                        line.startColor = Color.gray;
                                        line.endColor = Color.gray;
                                    }


                                playerManager_.conqueredBases.Remove(gameObject);

                                colorChanger.ChangeLineColor(collision.GetComponent<movePoint>().beginLine, collision.GetComponent<movePoint>().endLine, enemyColor);
                                gameObject.GetComponent<SpriteRenderer>().color = enemyColor;
                                if (!isEnamyBase && !transform.position.Equals(AIManager_.finishBase)
                                   /* && GameObject.FindGameObjectWithTag("AIController").GetComponent<AIManager>().conqueredBases.Count == GameObject.FindGameObjectWithTag("AIController").GetComponent<AIManager>().spheres.Count+1*/)
                                {
                                    GameObject point = Instantiate(enemyPoint, transform.position, Quaternion.identity);
                                    point.GetComponent<movePoint>().endLine = GameObject.FindGameObjectWithTag("antColony").GetComponent<AntColony>().ColonyWay(movePoint.EnemyNumber).Origin;
                                    point.GetComponent<movePoint>().goal = point.GetComponent<movePoint>().endLine;
                                    AIManager_.conqueredBases.Add(gameObject);
                                    AIManager_.spheres.Add(point);
                                    ++CurrentMaxIndexAI;
                                    indexForAI = CurrentMaxIndexAI;
                                }

                                isEnamyBase = true;

                                if (transform.position.Equals(AIManager_.finishBase))
                                {
                                    StartCoroutine(youLose());
                                }

                                //GameObject.FindGameObjectWithTag("AIController").GetComponent<AIManager>().playerLines.Add(new Assets.TestScripts.triangulation.Line(point.GetComponent<movePoint>().beginLine, transform.position));
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

    IEnumerator youWin()
    {
        yield return new WaitForSeconds(0.15f);
        endGame.SetActive(true);
        winFrame.SetActive(true);
        loseFrame.SetActive(false);
        pauseButton.SetActive(false);
        menuButton.SetActive(false);
        restartLevelButton.SetActive(false);
        //restartLevelButton.SetActive(true);
        if (Assets.TestScripts.triangulation.triangulation.level!=8)
        {
            nextLevelButton.SetActive(true);
            PlayerPrefs.SetInt("currentLevel", Assets.TestScripts.triangulation.triangulation.level + 1);
        }
        else
            menuButton.SetActive(true);

        //кінець гри
        Time.timeScale = 0;
    }

    IEnumerator youLose()
    {
        yield return new WaitForSeconds(0.15f);
        endGame.SetActive(true);
        winFrame.SetActive(false);
        loseFrame.SetActive(true);
        pauseButton.SetActive(false);
        menuButton.SetActive(false);
        //restartLevelButton.SetActive(true);
        nextLevelButton.SetActive(false);
        //кінець гри
        Time.timeScale = 0;
    }
}
