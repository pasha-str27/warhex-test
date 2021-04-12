using System.Collections;
using UnityEngine;

public class Base : MonoBehaviour
{
    public int playerScore;
    public int enemyScore;

    public Color playerColor;
    public Color enemyColor;

    public GameObject playerPoint;
    public GameObject enemyPoint;

    AIManager AIManager_;
    PlayerManager playerManager_;

    public GameObject conqueredBaseEffectByPlayer;
    public GameObject conqueredBaseEffectByComputer;

    int indexForAI;

    public bool isEnemyBase;

    static Animator menuAnimation;

    static GameObject endGame;
    static GameObject winFrame;
    static GameObject loseFrame;
    static GameObject pauseButton;
    static GameObject menuButton;
    static GameObject restartLevelButton;
    static GameObject nextLevelButton;

    private void Start()
    {
        if(menuAnimation == null)
        {
            menuAnimation = GameObject.FindWithTag("gameMenu").GetComponent<Animator>();
            menuAnimation.SetTrigger("startGame");
        }

        playerManager_ = GameObject.FindGameObjectWithTag("playerController").GetComponent<PlayerManager>();
        AIManager_ = GameObject.FindGameObjectWithTag("AIController").GetComponent<AIManager>();
        indexForAI = 0;
        isEnemyBase = false;
    }

    //line is conquered or not
    bool findLineInPlayerAndAI(LineRenderer line)
    {
        //searching line among player conquered lines
        foreach (var base_ in playerManager_.conqueredBases)
            if(new Vector2(base_.transform.position.x, base_.transform.position.y).Equals(line.GetPosition(0))
                || new Vector2(base_.transform.position.x, base_.transform.position.y).Equals(line.GetPosition(1)))
            return false;

        //searching line among player conquered lines
        foreach (var base_ in AIManager_.conqueredBases)
            if (new Vector2(base_.transform.position.x, base_.transform.position.y).Equals(line.GetPosition(0))
                || new Vector2(base_.transform.position.x, base_.transform.position.y).Equals(line.GetPosition(1)))
                return false;
        return true ;
    }

    //if point entered on trigger 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if base was conquered early then set 'zeroes' values for enemy
        //if (gameObject.tag == "Point")
        //{

            if (enemyScore == 10 && collision.GetComponent<movePoint>().isPlayer)
            {
                playerScore = 1;
                --enemyScore;
            }
            else
            {
                if (playerScore == 10 && !collision.GetComponent<movePoint>().isPlayer)
                {
                    --playerScore;
                    enemyScore = 1;
                }
                else
                {
                    //update enemy and player scores
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
                            if (playerScore > 0)
                                --playerScore;
                            ++enemyScore;
                        }

                        //if base was conquered by player
                        if (playerScore == 10 && enemyScore == 0)
                        {
                            Destroy(Instantiate(conqueredBaseEffectByPlayer), 1);

                            isEnemyBase = false;

                            //change color for lines
                            AntColony colorChanger = GameObject.FindGameObjectWithTag("antColony").GetComponent<AntColony>();

                            changeColorForLinesOnGray(colorChanger);

                            //if base was early conquared by computer
                            if (indexForAI > 0)
                            {
                                //then destroy all points which was spawn after conquared this base by computer

                                for (int i = AIManager_.conqueredBases.Count - 1; i >= indexForAI; --i)
                                {
                                    AIManager_.conqueredBases[i].GetComponent<SpriteRenderer>().color = Color.white;
                                    AIManager_.conqueredBases[i].GetComponent<Base>().enemyScore = 0;
                                    AIManager_.conqueredBases[i].GetComponent<Base>().isEnemyBase = false;
                                    AIManager_.conqueredBases[i].transform.GetChild(1).GetComponent<SpriteRenderer>().material.SetFloat("_Arc2", 360);
                                    AIManager_.conqueredBases.RemoveAt(i);
                                    Destroy(AIManager_.spheres[i - 1]);
                                    movePoint.EnemyNumber--;
                                    AIManager_.spheres.RemoveAt(i - 1);
                                }

                                //and change color for lines
                                foreach (var line in colorChanger.LineRenderersList)
                                    if (findLineInPlayerAndAI(line))
                                    {
                                        line.startColor = Color.gray;
                                        line.endColor = Color.gray;
                                    }
                            }

                            //change color for base
                            gameObject.GetComponent<SpriteRenderer>().color = playerColor;

                            //if base wasn't conquered early we add it to list on playerManager
                            if (playerManager_.conqueredBases.IndexOf(gameObject) == -1)
                                playerManager_.conqueredBases.Add((gameObject));

                            //and change color for conquered lines
                            for (int i = 0; i < playerManager_.conqueredBases.Count; ++i)
                                colorChanger.ChangeLineColor(gameObject.transform.position, playerManager_.conqueredBases[i].transform.position, playerColor);

                            indexForAI = 0;

                            //if it is finish base we make active winner panel
                            if (transform.position.Equals(playerManager_.finishBase))
                                StartCoroutine(youWin());
                        }
                        else
                        {
                            //if base was conquered by computer
                            if (enemyScore == 10 && playerScore == 0)
                            {
                                AntColony colorChanger = GameObject.FindGameObjectWithTag("antColony").GetComponent<AntColony>();
                                Destroy(Instantiate(conqueredBaseEffectByComputer), 1);

                                //change color for lines
                                changeColorForLinesOnGray(colorChanger);

                                //from player manager remove current base
                                playerManager_.conqueredBases.Remove(gameObject);

                            colorChanger.ChangeLineColor(collision.GetComponent<movePoint>().beginLine, collision.GetComponent<movePoint>().endLine, enemyColor);

                            if (transform.CompareTag("Point"))
                            {
                                //change color for base
                                gameObject.GetComponent<SpriteRenderer>().color = enemyColor;

                                //if was conquered new base
                                if (!isEnemyBase && !transform.position.Equals(AIManager_.finishBase))
                                {
                                    //spawn new point
                                    GameObject point = Instantiate(enemyPoint, transform.position, Quaternion.identity);
                                    point.GetComponent<movePoint>().endLine = GameObject.FindGameObjectWithTag("antColony").GetComponent<AntColony>().ColonyWay(movePoint.EnemyNumber).Origin;
                                    point.GetComponent<movePoint>().goal = point.GetComponent<movePoint>().endLine;
                                    point.GetComponent<movePoint>().speed *= PlayerPrefs.GetInt("difficulty");
                                    AIManager_.conqueredBases.Add(gameObject);
                                    AIManager_.spheres.Add(point);
                                    indexForAI = AIManager_.conqueredBases.Count - 1;
                                }
                            }
                                

                                isEnemyBase = true;

                                //if it is finish base then make active lose menu
                                if (transform.position.Equals(AIManager_.finishBase))
                                    StartCoroutine(youLose());
                            }
                        }
                    }
                }
            //}
            //update progress bar for base
            gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().material.SetFloat("_Arc1", 360 - playerScore * 36);
            gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>().material.SetFloat("_Arc2", 360 - enemyScore * 36);
        }

        //change direction for point
        if (collision.GetComponent<movePoint>().movingToEnemy)
            collision.GetComponent<movePoint>().goal = collision.GetComponent<movePoint>().beginLine;
        else
            collision.GetComponent<movePoint>().goal = collision.GetComponent<movePoint>().endLine;

        collision.GetComponent<movePoint>().movingToEnemy = !collision.GetComponent<movePoint>().movingToEnemy;
    }

    //change color for lines which is not conquered
    void changeColorForLinesOnGray(AntColony colorChanger)
    {
        foreach (var line in colorChanger.LineRenderersList)
            if (new Vector2(line.GetPosition(0).x, line.GetPosition(0).y).Equals(transform.position) || new Vector2(line.GetPosition(1).x, line.GetPosition(1).y).Equals(transform.position))
            {
                line.startColor = Color.gray;
                line.endColor = Color.gray;
            }
    }

    //show win panel
    IEnumerator youWin()
    {
        movePoint.letMovePoint = false;
        yield return new WaitForSeconds(0.3f);

        if (Assets.Scripts.triangulation.triangulation.level!=20)
        {
            menuAnimation.SetTrigger("levelComplete");
            PlayerPrefs.SetInt("currentLevel", Assets.Scripts.triangulation.triangulation.level + 1);
        }
        else
            menuAnimation.SetTrigger("lastLevelComplete");

        Time.timeScale = 0;
    }

    //game over
    IEnumerator youLose()
    {
        movePoint.letMovePoint = false;
        yield return new WaitForSeconds(0.3f);
        menuAnimation.SetTrigger("gameOver");
        Time.timeScale = 0;
    }
}
