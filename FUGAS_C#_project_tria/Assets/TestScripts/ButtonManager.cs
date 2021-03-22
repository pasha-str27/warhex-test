using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public static bool ShowCreatorInfo = false;

    public Animator transition;
    public float transitionTime = 1f;

    public GameObject levelSubmenu;

    public GameObject clickOnButtonEffect;

    public GameObject[] audioEffects;
    public GameObject audioVolumeChnger;

    public GameObject AngelinaSound;

    static bool firstStart;

    static bool wasClickOnLogo;

    static float soundLevelOnMenu;


    private void Start()
    {
        if (!firstStart)
        {
            audioController.lvlsound.GetComponent<AudioSource>().volume = 0.3f;
            soundLevelOnMenu = audioController.lvlsound.GetComponent<AudioSource>().volume;
            audioVolumeChnger.GetComponent<Slider>().value = soundLevelOnMenu;
            for (int i = 0; i < audioEffects.Length; ++i)
                audioEffects[i].GetComponent<AudioSource>().volume = audioVolumeChnger.GetComponent<Slider>().value;
            audioController.lvlsound.GetComponent<AudioSource>().volume = audioVolumeChnger.GetComponent<Slider>().value;

            firstStart = true;
        }
        else
            if (audioVolumeChnger != null)
            audioVolumeChnger.GetComponent<Slider>().value = audioController.lvlsound.GetComponent<AudioSource>().volume;
    }

    public void changeVolumeLevel()
    {
        for (int i = 0; i < audioEffects.Length; ++i)
            audioEffects[i].GetComponent<AudioSource>().volume = audioVolumeChnger.GetComponent<Slider>().value;
        audioController.lvlsound.GetComponent<AudioSource>().volume = audioVolumeChnger.GetComponent<Slider>().value;
    }

    public void Play()
    {
        EventSystem.current.SetSelectedGameObject(null);
        StartCoroutine(LoadLevel("Game"));
    }

    public void ShowCreatorsInfo()
    {
        EventSystem.current.SetSelectedGameObject(null);
        wasClickOnLogo = true;
        StartCoroutine(LoadLevel("Credits"));
    }

    private IEnumerator LoadLevel(string levelName)
    {
        //transition.SetTrigger("Start");

        //yield return new WaitForSeconds(transitionTime);

        //SceneManager.LoadScene(levelName);
        transition.SetTrigger("Start");
        if(levelName=="Game")
        {
            soundLevelOnMenu = audioController.lvlsound.GetComponent<AudioSource>().volume;
            int i = 50;
            float h = (soundLevelOnMenu - 0.025f) / i;
            for (; i >= 0; --i)
            {
                yield return new WaitForSeconds(Time.deltaTime);
                audioController.lvlsound.GetComponent<AudioSource>().volume -= h;
            }
        }

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelName);
    }

    public void MainMenu()
    {
        GameIsPaused = false;
        Debug.Log("Game Unpaused");
        Time.timeScale = 1f;
        StartCoroutine(LoadMenu());
        //SceneManager.LoadScene("MainMenu");
        EventSystem.current.SetSelectedGameObject(null);
    }

    private IEnumerator LoadMenu()
    {
        Time.timeScale = 1f;
        transition.SetTrigger("Start");
        if(!wasClickOnLogo)
        {
            int i = 100;
            float h = (soundLevelOnMenu - audioController.lvlsound.GetComponent<AudioSource>().volume) / i;
            for (; i > 0; --i)
            {
                yield return null;
                audioController.lvlsound.GetComponent<AudioSource>().volume += h;
            }
        }
        else
        {
            int i = 100;
            float h = AngelinaSound.GetComponent<AudioSource>().volume / i;
            for (; i >= 0; --i)
            {
                yield return new WaitForSeconds(Time.deltaTime);
                audioController.lvlsound.GetComponent<AudioSource>().volume += h;
                AngelinaSound.GetComponent<AudioSource>().volume -= h;
            }
            //yield return new WaitForSeconds(transitionTime);
        }
            
        wasClickOnLogo = false;
        SceneManager.LoadScene(0);
    }

    public void RestartLevel()
    {
        GameIsPaused = false;
        movePoint.EnemyNumber = 0;
        Debug.Log("Reloading level");
        Time.timeScale = 1f;
        //Assets.TestScripts.triangulation.triangulation.level = level;
        Assets.TestScripts.triangulation.triangulation.loadingFromLevelsMenu = true;

        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void nextLevel()
    {
        GameIsPaused = false;
        movePoint.EnemyNumber = 0;
        Debug.Log("Reloading level");
        Time.timeScale = 1f;
        //Assets.TestScripts.triangulation.triangulation.level = level;
        //Assets.TestScripts.triangulation.triangulation.loadingFromLevelsMenu = true;

        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void loadLevel(int level)
    {
        movePoint.EnemyNumber = 0;
        Debug.Log("Reloading level");
        Time.timeScale = 1f;
        Assets.TestScripts.triangulation.triangulation.level = level;
        Assets.TestScripts.triangulation.triangulation.loadingFromLevelsMenu = true;
        StartCoroutine(LoadLevel("Game"));
        //SceneManager.LoadScene(2);
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

    public void playAudioEffect()
    {
        Destroy(Instantiate(clickOnButtonEffect), 1);
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
