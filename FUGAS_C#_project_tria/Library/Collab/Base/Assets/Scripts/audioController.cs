using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class audioController : MonoBehaviour
{
    public static GameObject lvlsound;

    //creating Don'tDestroyOnLoad GameObject it is background music 
    void Awake()
    {
        Time.timeScale = 1f;
        if (lvlsound == null)
        {
            lvlsound = GameObject.Find("sound");
            if (lvlsound == null)
                lvlsound = (GameObject)Instantiate(Resources.Load("sound"));
            lvlsound.name = "sound";
            DontDestroyOnLoad(lvlsound);
        }
    }

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Credits")
            StartCoroutine(changeSoundLevel());
    }

    //change volume level for backgrounds sounds
    IEnumerator changeSoundLevel()
    {
        int i = 100;
        float h = (lvlsound.GetComponent<AudioSource>().volume) / i;
        for (; i >= 0; --i)
        {
            yield return new WaitForSeconds(Time.deltaTime * 2);
            lvlsound.GetComponent<AudioSource>().volume -= h;

            GetComponent<AudioSource>().volume += h;
        }
    }
}

