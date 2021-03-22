using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonManager : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public static bool ShowCreatorInfo = false;

    public Animator transition;
    public float transitionTime = 1f;

    public GameObject levelSubmenu;

    public void Play()
    {
        EventSystem.current.SetSelectedGameObject(null);
        StartCoroutine(LoadLevel("Game"));
    }

    private IEnumerator LoadLevel(string levelName)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelName);
    }

    public void MainMenu()
    {
        GameIsPaused = false;
        Debug.Log("Game Unpaused");
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void RestartLevel()
    {
        GameIsPaused = false;
        movePoint.EnemyNumber = 0;
        Debug.Log("Reloading level");
        Time.timeScale = 1f;
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void Pause()
    {
        if (GameIsPaused)
        {
            Debug.Log("Game Unpaused");
            GameIsPaused = false;
            Time.timeScale = 1f;
        }
        else
        {
            Debug.Log("Game Paused");
            GameIsPaused = true;
            Time.timeScale = 0f;
        }
        //EventSystem.current.SetSelectedGameObject(null);
    }

    public void QuitGame()
    {
        Debug.Log("Game Closed");
        Application.Quit();
    }

    public void LogoClick()
    {
        if (ShowCreatorInfo)
        {
            Debug.Log("Showing creator info");
            ShowCreatorInfo = false;
        }
        else
        {
            Debug.Log("Creator info is hidden");
            ShowCreatorInfo = true;
        }
    }
}
