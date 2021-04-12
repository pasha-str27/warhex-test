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

    static AIManager AIManager_;
    static PlayerManager playerManager_;

    public GameObject conqueredBaseEffectByPlayer;
    public GameObject conqueredBaseEffectByComputer;

    int indexForAI;

    public bool isEnemyBase;

    static Animator menuAnimation;
    static AntColony _antColony;

    public int maxPointCounter = 10;
    public int lastLevel = 20;

    private void Start()
    {
        if(_antColony == null)
            _antColony = GameObject.FindGameObjectWithTag("antColony").GetComponent<AntColony>();

        if (menuAnimation == null)
        {
            menuAnimation = GameObject.FindWithTag("gameMenu").GetComponent<Animator>();
            menuAnimation.SetTrigger("startGame");
        }

        if(playerManager_==null)
        {
            playerManager_ = GameObject.FindGameObjectWithTag("playerController").GetComponent<PlayerManager>();
            AIManager_ = GameObject.FindGameObjectWithTag("AIController").GetComponent<AIManager>();
        }

        indexForAI = 0;
        isEnemyBase = false;
    }

    //line is conquered or not
    bool findLineInPlayerAndAI(LineRenderer line)
    {
        var linePos0 = line.GetPosition(0);
        var linePos1 = line.GetPosition(1);
        //searching line among player conquered lines
        foreach (var base_ in playerManager_.conqueredBases)
        {
            Vector3 basePos = new Vector2(base_.transform.position.x, base_.transform.position.y);

            if (basePos.Equals(linePos0) || basePos.Equals(linePos1))
                return false;
        }


        //searching line among player conquered lines
        foreach (var base_ in AIManager_.conqueredBases)
        {
            Vector3 basePos = new Vector2(base_.transform.position.x, base_.transform.position.y);

            if (basePos.Equals(linePos0) || basePos.Equals(linePos1))
                return false;
        }

        return true;
    }

    //if point entered on trigger 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if base was conquered early then set 'zeroes' values for enemy

        movePoint movePoint_ = collision.GetComponent<movePoint>();
        Vector3 pos = transform.position;

        if (enemyScore == maxPointCounter && movePoint_.isPlayer)
        {
            playerScore = 1;
            --enemyScore;
        }
        else
        {
            if (playerScore == maxPointCounter && !movePoint_.isPlayer)
            {
                --playerScore;
                enemyScore = 1;
            }
            else
            {
                //update enemy and player scores
                if (enemyScore <= maxPointCounter - 1 && playerScore <= maxPointCounter - 1)
                {
                    if (movePoint_.isPlayer)
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
                    if (playerScore == maxPointCounter && enemyScore == 0)
                    {
                        Destroy(Instantiate(conqueredBaseEffectByPlayer), 1);

                        isEnemyBase = false;

                        //change color for lines

                        changeColorForLinesOnGray(_antColony);

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
                            foreach (var line in _antColony.LineRenderersList)
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
                            _antColony.ChangeLineColor(pos, playerManager_.conqueredBases[i].transform.position, playerColor);

                        indexForAI = 0;

                        //if it is finish base we make active winner panel
                        if (pos.Equals(playerManager_.finishBase))
                            StartCoroutine(youWin());
                    }
                    else
                    {
                        //if base was conquered by computer
                        if (enemyScore == maxPointCounter && playerScore == 0)
                        {
                            Destroy(Instantiate(conqueredBaseEffectByComputer), 1);

                            //change color for lines
                            changeColorForLinesOnGray(_antColony);

                            //from player manager remove current base
                            playerManager_.conqueredBases.Remove(gameObject);

                            _antColony.ChangeLineColor(movePoint_.beginLine, movePoint_.endLine, enemyColor);

                            if (transform.CompareTag("Point"))
                            {
                                //change color for base
                                gameObject.GetComponent<SpriteRenderer>().color = enemyColor;

                                //if was conquered new base
                                if (!isEnemyBase && !pos.Equals(AIManager_.finishBase))
                                {
                                    //spawn new point
                                    GameObject point = Instantiate(enemyPoint, transform.position, Quaternion.identity);

                                    var movePoint__ = point.GetComponent<movePoint>();

                                    movePoint__.endLine = _antColony.ColonyWay(movePoint.EnemyNumber).Origin;
                                    movePoint__.goal = point.GetComponent<movePoint>().endLine;
                                    movePoint__.speed *= PlayerPrefs.GetInt("difficulty");
                                    AIManager_.conqueredBases.Add(gameObject);
                                    AIManager_.spheres.Add(point);
                                    indexForAI = AIManager_.conqueredBases.Count - 1;
                                }
                            }  

                            isEnemyBase = true;

                            //if it is finish base then make active lose menu
                            if (pos.Equals(AIManager_.finishBase))
                                StartCoroutine(youLose());
                        }
                    }
                }
            }
            //update progress bar for base
            gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().material.SetFloat("_Arc1", 360 - playerScore * (360 / maxPointCounter));
            gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>().material.SetFloat("_Arc2", 360 - enemyScore * (360 / maxPointCounter));
        }

        //change direction for point
        if (movePoint_.movingToEnemy)
            movePoint_.goal = movePoint_.beginLine;
        else
            movePoint_.goal = movePoint_.endLine;

        movePoint_.movingToEnemy = !movePoint_.movingToEnemy;
    }

    //change color for lines which is not conquered
    void changeColorForLinesOnGray(AntColony colorChanger)
    {
        foreach (var line in colorChanger.LineRenderersList)
        {
            Vector3 pos0 = line.GetPosition(0);
            Vector3 pos1 = line.GetPosition(1);
            if (new Vector2(pos0.x, pos0.y).Equals(transform.position)
                || new Vector2(pos1.x, pos1.y).Equals(transform.position))
            {
                line.startColor = Color.gray;
                line.endColor = Color.gray;
            }
        }
    }

    //show win panel
    IEnumerator youWin()
    {
        yield return new WaitForSeconds(0.3f);

        if (Assets.Scripts.triangulation.triangulation.level!=lastLevel)
        {
            menuAnimation.SetTrigger("levelComplete");
            PlayerPrefs.SetInt("currentLevel", Assets.Scripts.triangulation.triangulation.level + 1);
        }
        else
            menuAnimation.SetTrigger("lastLevelComplete");

        movePoint.letMovePoint = false;
    }

    //game over
    IEnumerator youLose()
    {
        movePoint.letMovePoint = false;
        yield return new WaitForSeconds(0.3f);
	    menuAnimation.SetTrigger("gameOver");
    }
}
