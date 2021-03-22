using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class audioController : MonoBehaviour
{
    public static GameObject lvlsound;
    void Awake()
    {
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

