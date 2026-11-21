using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelSelect : MonoBehaviour
{
    public void OpenLevel(int levelID)
    {
        SceneManager.LoadScene(levelID);
        //need check scene manager to see id and make sure loads correct game scene
    }
}
