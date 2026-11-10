using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelExit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Exit trigger touched by: " + collision.name + " (tag: " + collision.tag + ")");
        if (!collision.CompareTag("Player")) return;

        GameManager gm = GameManager.Instance;

        if(gm.docsCollected >= gm.docsNeeded)
        {
            Debug.Log("Level Passed");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);// basically checks secen then increases to next when player reaches the scene
            
            //pass in build index for level want to change to
        }
        if (gm.docsKept < gm.docsNeeded)
        {
            Debug.Log("You need to keep the document first! (" + gm.docsKept + "/" + gm.docsNeeded + ")");
            return;
        }
    }




}
