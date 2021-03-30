using UnityEngine;
using UnityEngine.UI;

public class checkAvailableLevel : MonoBehaviour
{
    public int level;

    void Start()
    {
        //is it is first game then available only first level
        if (!PlayerPrefs.HasKey("currentLevel"))
            PlayerPrefs.SetInt("currentLevel", 1);

        //remove enabled to level button
        if (level > PlayerPrefs.GetInt("currentLevel"))
        {
            GetComponent<Button>().enabled = false;
            transform.GetChild(0).GetComponent<Text>().color = new Color(128f / 255f, 65f / 255f, 65f / 255f);
        }
    }
}
