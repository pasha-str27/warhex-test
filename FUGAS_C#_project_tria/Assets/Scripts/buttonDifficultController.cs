using UnityEngine;
using UnityEngine.UI;

public class buttonDifficultController : MonoBehaviour
{
    public int dificultLevel;
    static Image dificultyImage;

    //searching difficulty level
    void Start()
    {
        if (PlayerPrefs.GetInt("dificulty") == dificultLevel)
        {
            GetComponent<Image>().color = Color.red;
            dificultyImage = GetComponent<Image>();
        }     
    }

    //change diffilty level
    public void changeDificulty()
    {
        dificultyImage.color = Color.white;
        GetComponent<Image>().color = Color.red;
        dificultyImage = GetComponent<Image>();
        PlayerPrefs.SetInt("dificulty", dificultLevel);
    }
}
