using UnityEngine;


//script going to be dealing with rules and controlling game state
public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    bool gameOver;
    public int docsCollected;
    public int docsNeeded = 1;

    //public int playerSus = 0;
    //public int maxPlayerSus = 15;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Instance = this; // lets any script access the GameManager instance
    }

    void Win()
    {
        gameOver = true;
        Debug.Log("You win!");
    }
}
