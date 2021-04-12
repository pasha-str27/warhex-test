using UnityEngine;
using UnityEngine.UI;

public class checkAvailableLevel : MonoBehaviour
{
    public int level;
    public Color unavailableColor;

    void Start()
    {
        //is it is first game then available only first level
        if (!PlayerPrefs.HasKey("currentLevel"))
            PlayerPrefs.SetInt("currentLevel", 1);

        //remove enabled to level button
        if (level > PlayerPrefs.GetInt("currentLevel"))
        {
            GetComponent<Button>().enabled = false;
            transform.GetComponent<Text>().color = unavailableColor;
        }
    }
}
