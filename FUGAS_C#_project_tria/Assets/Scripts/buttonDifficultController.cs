using UnityEngine;
using UnityEngine.UI;

public class buttonDifficultController : MonoBehaviour
{
    public int difficultyLevel;
    static Image difficultyImage;

    //searching difficulty level
    void Start()
    {
        if (PlayerPrefs.GetInt("difficulty") == difficultyLevel)
        {
            GetComponent<Image>().color = Color.red;
            difficultyImage = GetComponent<Image>();
        }     
    }

    //change diffilty level
    public void changedifficulty()
    {
        difficultyImage.color = Color.white;
        difficultyImage = GetComponent<Image>();
        difficultyImage.color = Color.red;

        PlayerPrefs.SetInt("difficulty", difficultyLevel);
    }
}
