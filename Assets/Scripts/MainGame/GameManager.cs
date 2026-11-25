using UnityEngine;
using UnityEngine.SceneManagement;
public enum GameState { Playing, Working, GameOver, Won, Pause }
//script going to be dealing with rules and controlling game state


//enums for room types and restrictions
public enum roomRestrictions {None = 0, Work = 1, Sprint = 2, Collect = 4 }

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    bool gameOver;
    public int docsCollected;
    public int docsKept;
    public int docsToReview;

    public int docsNeeded = 1;
    
    public bool IsGameOver => gameOver;

    [SerializeField] GameObject papersPleaseUI;   // the PapersPlease object under your UI canvas
    public bool IsWorking { get; private set; }
    public bool IsUsingKeypad { get; private set; }   // true while a door keypad is open (player stands still)




    // counters, not bools, so overlapping rooms can't unblock each other by mistake
    int workBlocks, sprintBlocks, collectBlocks;

    public bool CanWork => workBlocks == 0;      // covers both using the desk and reviewing documents
    public bool CanSprint => sprintBlocks == 0;
    public bool CanCollect => collectBlocks == 0;



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
        if (gameOver || !CanWork) working = false;   // never open doc comparison if blocked or game/ level complete

        IsWorking = working;

        if (papersPleaseUI) papersPleaseUI.SetActive(working);

        // let the drag scripts know. They only allow dragging while the state is Working.
        if (DocumentManager.instance != null)
            DocumentManager.instance.SetState(working ? EState.Working : EState.Start);   // Start is the "idle" state

        if (working && DocumentManager.instance != null)
            DocumentManager.instance.OpenDesk();
    }

    // called by DoorKeypadUI when the keypad opens or closes
    public void SetUsingKeypad(bool open)
    {
        if (gameOver) open = false;
        IsUsingKeypad = open;
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

    public void DocumentPickUp()
    {
        docsCollected++;
        docsToReview++;
    }


    public bool KeepDocument()
    {
        if (docsToReview <= 0) { return false; }
        docsToReview--;
        docsKept++;
        return true;
    }

    public bool DiscardDocument()
    {
        if (docsToReview <= 0) { return false; }
        docsToReview--;
        return true;
    }


    //fucntion to add restriction blocks too a room subclass
    public void AddRestrictions(roomRestrictions r)
    {
        if (r.HasFlag(roomRestrictions.Work)) workBlocks++;
        if (r.HasFlag(roomRestrictions.Sprint)) sprintBlocks++;
        if (r.HasFlag(roomRestrictions.Collect)) collectBlocks++;

        if (!CanWork && IsWorking) SetWorking(false);   // close the desk if it was open
    }

    //helper function to remove restrictions from room
    public void RemoveRestrictions(roomRestrictions r)
    {
        if (r.HasFlag(roomRestrictions.Work)) workBlocks = Mathf.Max(0, workBlocks - 1);
        if (r.HasFlag(roomRestrictions.Sprint)) sprintBlocks = Mathf.Max(0, sprintBlocks - 1);
        if (r.HasFlag(roomRestrictions.Collect)) collectBlocks = Mathf.Max(0, collectBlocks - 1);
    }

}
