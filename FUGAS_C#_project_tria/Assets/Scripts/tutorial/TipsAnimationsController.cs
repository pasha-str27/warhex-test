using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipsAnimationsController : MonoBehaviour
{
    public GameObject tipsFrame;
    private CanvasGroup _canvasGroup;
    public static int _currentAnimation;
    public Animator _animations;
    static public bool playingAnimation;

    void Start()
    {
        _currentAnimation = 1;
        _canvasGroup = tipsFrame.GetComponent<CanvasGroup>();
        //StartCoroutine(AnimationStart());
    }

    private void Update()
    {
        if(Input.GetMouseButton(0)&&!playingAnimation&& _currentAnimation!=5 && _currentAnimation != 7
            && _currentAnimation != 11 && _currentAnimation != 13 && _currentAnimation != 14)
        {
            playingAnimation = true;
            StartCoroutine(AnimationEnd());
        }
    }

    public IEnumerator AnimationStart()
    {
        Debug.Log("Started tip animation");
        playingAnimation = true;
        while (_canvasGroup.alpha < 1)
        {
            _canvasGroup.alpha += 0.01f;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        playingAnimation = false;
    }
    public IEnumerator AnimationEnd()
    {
        Debug.Log("Finished tip animation");
        _currentAnimation += 1;
        playingAnimation = true;
        while (_canvasGroup.alpha > 0)
        {
            _canvasGroup.alpha -= 0.01f;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        playingAnimation = false;
        _animations.SetTrigger("step" + _currentAnimation.ToString());

        if (_currentAnimation != 5&& _currentAnimation != 7 && _currentAnimation != 11)
            StartCoroutine(AnimationStart());
    }
}
