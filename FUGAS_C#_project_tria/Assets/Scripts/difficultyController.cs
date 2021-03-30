using UnityEngine;

public class difficultyController : MonoBehaviour
{
    void Start()
    {
        //search button with current level of difficulty
        if (!PlayerPrefs.HasKey("dificulty"))
            PlayerPrefs.SetInt("dificulty", 1);
    }
}
