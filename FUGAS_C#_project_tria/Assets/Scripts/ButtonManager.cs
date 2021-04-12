using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public bool ShowCreatorInfo = false;
    public Animator transition;
    public float transitionTime = 1f;

    public GameObject clickOnButtonEffect;
    public AudioSource[] audioEffects;
    public GameObject audioVolumeChanger;
    public GameObject AngelinaSound;
    static bool wasClickOnLogo;
    static float soundLevelOnMenu;
    float _maxAudioLevel = 0.3f;

    public static bool loadingFromMenu;

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
            audioEffects[i].volume = soundLevelOnMenu;
    }

    public void setLoadingFromMenu(bool value)
    {
        loadingFromMenu = value;
    }

    public void changeVolumeLevel()
    {
        float newValue = audioVolumeChanger.GetComponent<Slider>().value;
        for (int i = 0; i < audioEffects.Length; ++i)
            audioEffects[i].volume = newValue;
        audioController.lvlsound.GetComponent<AudioSource>().volume = newValue;
        PlayerPrefs.SetFloat("audioLevel", newValue);
    }

    public void Play()
    {
        EventSystem.current.SetSelectedGameObject(null);
        if (PlayerPrefs.HasKey("currentLevel"))
            StartCoroutine(LoadLevel("Game"));
        else
            Tutorial();
    }

    public void Tutorial()
    {
        EventSystem.current.SetSelectedGameObject(null);
        StartCoroutine(LoadLevel("Tutorial"));
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
        if(levelName=="Game"|| levelName=="Tutorial")
        {
            if(loadingFromMenu)
            {
                int i = 50;
                float h = 2 * (soundLevelOnMenu / 3) / i;
                var audio = audioController.lvlsound.GetComponent<AudioSource>();
                for (; i >= 0; --i)
                {
                    yield return new WaitForSeconds(Time.deltaTime);
                    audio.volume -= h;
                }
            }
            else
                yield return new WaitForSeconds(transitionTime);
        }

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelName);
    }

    public void MainMenu()
    {
        Debug.Log("Game Unpaused");
        StartCoroutine(LoadMenu());
        EventSystem.current.SetSelectedGameObject(null);
    }

    private IEnumerator LoadMenu()
    {
        movePoint.letMovePoint = false;
        transition.SetTrigger("Start");

        var soundOnMenu = audioController.lvlsound.GetComponent<AudioSource>();
        if (!wasClickOnLogo)
        {
            int i = 100;
            float h = (soundLevelOnMenu - soundOnMenu.volume) / i;
            for (; i > 0; --i)
            {
                yield return new WaitForSeconds(Time.deltaTime);
                soundOnMenu.volume += h;
            }
        }
        else
        {
            int i = 100;
            var angelinaSound = AngelinaSound.GetComponent<AudioSource>();
            float h = angelinaSound.volume / i;
            for (; i >= 0; --i)
            {
                yield return new WaitForSeconds(Time.deltaTime);
                soundOnMenu.volume += h;
                angelinaSound.volume -= h;
            }
        }
            
        wasClickOnLogo = false;
        SceneManager.LoadScene(0);
    }

    public void RestartLevel()
    {
        movePoint.EnemyNumber = 0;
        Debug.Log("Reloading level");
        movePoint.letMovePoint = true;
        Assets.Scripts.triangulation.triangulation.loadingFromLevelsMenu = true;

        int scene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scene);
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void nextLevel()
    {
        movePoint.EnemyNumber = 0;
        Debug.Log("Reloading level");
        EventSystem.current.SetSelectedGameObject(null);

        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void loadLevel(int level)
    {
        movePoint.EnemyNumber = 0;
        Debug.Log("Reloading level");
        movePoint.letMovePoint = true;
        Assets.Scripts.triangulation.triangulation.level = level;
        Assets.Scripts.triangulation.triangulation.loadingFromLevelsMenu = true;
        EventSystem.current.SetSelectedGameObject(null);
        StartCoroutine(LoadLevel("Game"));
    }

    public void Pause()
    {
        EventSystem.current.SetSelectedGameObject(null);
        if (!movePoint.letMovePoint)
        {
            Debug.Log("Game Unpaused");
            movePoint.letMovePoint = true;
        }
        else
        {
            Debug.Log("Game Paused");
            movePoint.letMovePoint = false;
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
