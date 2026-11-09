using UnityEngine;
using UnityEngine.SceneManagement;
public enum GameState { Playing, Working, GameOver, Won, Pause }
//script going to be dealing with rules and controlling game state
public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    bool gameOver;
    public int docsCollected;
    public int docsNeeded = 1;
    public int docsKept;
    public bool IsGameOver => gameOver;

    [SerializeField] GameObject papersPleaseUI;   // the PapersPlease object under your UI canvas
    public bool IsWorking { get; private set; }

    //public GameState State { get; private set; } = GameState.Playing;
    //public int casesCorrect, casesWrong;
    //public int maxMistakes = 3;

    //public void SetState(GameState s) => State = s;

    void Awake()
    {
        Instance = this; // lets any script access the GameManager instance
        Time.timeScale = 1f;   // timeScale carries over between scene loads, so reset it
    }


    void Start()
    {
        if (papersPleaseUI) papersPleaseUI.SetActive(false);   // starts hidden
    }

    public void PlayerCaught()
    {
        if (gameOver) return;
        gameOver = true;
        Debug.Log("Caught by a guard! Game over. Press R to restart.");
        Time.timeScale = 0f;   // freezes physics, the guard and the suspicion meter
    }

    void Update()
    {
        if (gameOver && Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //to activate and deactivate the UI for the papers please
    public void SetWorking(bool working)
    {
        if (gameOver) working = false;   // never open the desk after winning or losing

        IsWorking = working;

        if (papersPleaseUI) papersPleaseUI.SetActive(working);

        // let the drag scripts know. They only allow dragging while the state is Working.
        if (DocumentManager.instance != null)
            DocumentManager.instance.SetState(working ? EState.Working : EState.Start);   // Start is the "idle" state
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
