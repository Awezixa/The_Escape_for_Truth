using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LevelSelect : MonoBehaviour
{

    public Button[] buttons;


    //For locking and unlocking of levels
    private void Awake()
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        for(int i =0; i< buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }
        for (int i = 0; i < unlockedLevel; i++)
        {
            buttons[i].interactable = true;
        }
    }


    public void OpenLevel(int levelID)
    {
        SceneManager.LoadScene(levelID);
        //need check scene manager to see id and make sure loads correct game scene
    }
}
