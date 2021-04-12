using System.Collections;
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

    public GameObject clickOnButtonEffect;

    public GameObject[] audioEffects;
    public GameObject audioVolumeChanger;

    public GameObject AngelinaSound;

    static bool wasClickOnLogo;

    static float soundLevelOnMenu;

    float _maxAudioLevel = 0.3f;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("audioLevel"))
            PlayerPrefs.SetFloat("audioLevel", _maxAudioLevel);
        soundLevelOnMenu = PlayerPrefs.GetFloat("audioLevel");

        if (audioVolumeChanger != null)
        {
            audioVolumeChanger.GetComponent<Slider>().value = soundLevelOnMenu;
            audioController.lvlsound.GetComponent<AudioSource>().volume = soundLevelOnMenu;
        }

        for (int i = 0; i < audioEffects.Length; ++i)
            audioEffects[i].GetComponent<AudioSource>().volume = soundLevelOnMenu;
    }

    public void changeVolumeLevel()
    {
        float newValue = audioVolumeChanger.GetComponent<Slider>().value;
        for (int i = 0; i < audioEffects.Length; ++i)
            audioEffects[i].GetComponent<AudioSource>().volume = newValue;
        audioController.lvlsound.GetComponent<AudioSource>().volume = newValue;
        PlayerPrefs.SetFloat("audioLevel", newValue);
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
        transition.SetTrigger("Start");
        if(levelName=="Game")
        {
            int i = 50;
            float h = 2*(soundLevelOnMenu/3) / i;
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
                yield return new WaitForSeconds(Time.deltaTime);
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
        Assets.Scripts.triangulation.triangulation.loadingFromLevelsMenu = true;

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

        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void loadLevel(int level)
    {
        movePoint.EnemyNumber = 0;
        Debug.Log("Reloading level");
        Time.timeScale = 1f;
        Assets.Scripts.triangulation.triangulation.level = level;
        Assets.Scripts.triangulation.triangulation.loadingFromLevelsMenu = true;
        StartCoroutine(LoadLevel("Game"));
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
