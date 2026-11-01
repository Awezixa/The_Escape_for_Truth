using UnityEngine;

public enum GameState { Playing, Working, GameOver, Won }
//script going to be dealing with rules and controlling game state
public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    bool gameOver;
    public int docsCollected;
    public int docsNeeded = 1;

    //public int playerSus = 0;
    //public int maxPlayerSus = 15;

    //public GameState State { get; private set; } = GameState.Playing;
    //public int casesCorrect, casesWrong;
    //public int maxMistakes = 3;

    //public void SetState(GameState s) => State = s;



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
    //public void CaseResolved(bool correct)
    //{
    //    if (correct) { casesCorrect++; return; }
    //    casesWrong++;
    //    // later: playerSus += 5;
    //    if (casesWrong >= maxMistakes) { State = GameState.GameOver; Debug.Log("Fired!"); }
    //}
}
