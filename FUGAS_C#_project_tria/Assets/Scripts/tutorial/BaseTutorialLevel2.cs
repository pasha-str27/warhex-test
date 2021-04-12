using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTutorialLevel2 : MonoBehaviour
{
    public int playerScore;
    public int enemyScore;

    public Color playerColor;
    public Color enemyColor;

    static PlayerControllerTutorial playerManager_;

    public GameObject conqueredBaseEffectByPlayer;
    public GameObject conqueredBaseEffectByComputer;

    static Animator menuAnimation;

    public int maxPointCounter = 10;

    static TipsAnimationsController _animationController;

    private void Start()
    {
        if (_animationController == null)
            _animationController = GameObject.FindGameObjectWithTag("animation").GetComponent<TipsAnimationsController>();
        if (playerManager_ == null)
            playerManager_ = GameObject.FindGameObjectWithTag("playerController").GetComponent<PlayerControllerTutorial>();
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

        return true;
    }

    //if point entered on trigger 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if base was conquered early then set 'zeroes' values for enemy

        movePoint movePoint_ = collision.GetComponent<movePoint>();
        Vector2 pos = transform.position;

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

                        //change color for base
                        gameObject.GetComponent<SpriteRenderer>().color = playerColor;

                        //if base wasn't conquered early we add it to list on playerManager
                        if (playerManager_.conqueredBases.IndexOf(gameObject) == -1)
                            playerManager_.conqueredBases.Add((gameObject));

                        //and change color for conquered lines
                        for (int i = 0; i < playerManager_.conqueredBases.Count; ++i)
                            playerManager_.ChangeLineColor(pos, playerManager_.conqueredBases[i].transform.position, playerColor);

                        //if it is finish base we make active winner panel
                        if (pos.Equals(playerManager_.finishBase))
                            StartCoroutine(youWin());
                    }
                    else
                    {
                        //if base was conquered by computer
                        if (enemyScore == maxPointCounter && playerScore == 0)
                        {
                            if(conqueredBaseEffectByComputer!=null)
                                Destroy(Instantiate(conqueredBaseEffectByComputer), 1);

                            //from player manager remove current base
                            playerManager_.conqueredBases.Remove(gameObject);

                            playerManager_.ChangeLineColor(movePoint_.beginLine, movePoint_.endLine, enemyColor);
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

    IEnumerator youWin()
    {
        movePoint.letMovePoint = false;

        yield return new WaitForSeconds(0.5f);
        TipsAnimationsController._currentAnimation++;
        StartCoroutine(_animationController.AnimationStart());
        _animationController._animations.SetTrigger("step" + TipsAnimationsController._currentAnimation.ToString());

        if (TipsAnimationsController._currentAnimation == 14 && !PlayerPrefs.HasKey("currentLevel"))
            PlayerPrefs.SetInt("currentLevel", 1);

        for (int i=0;i<150;++i)
        {
            TipsAnimationsController.playingAnimation = true;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        
        TipsAnimationsController.playingAnimation = false;
        if (TipsAnimationsController._currentAnimation == 12)
            Destroy(transform.parent.gameObject);
    }
}
