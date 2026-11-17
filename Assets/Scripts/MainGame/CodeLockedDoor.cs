using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class CodeLockedDoor : MonoBehaviour
{
    [SerializeField] string code = "0427";
    [SerializeField] Collider2D doorBlocker;
    [SerializeField] SpriteRenderer doorSprite;

    bool playerNearby;
    bool hasCode;
    bool enteringCode;
    bool unlocked;
    bool wrongCode;
    string enteredCode = "";
    float showNoteUntil;

    void Reset()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    public void CollectCode()
    {
        hasCode = true;
        showNoteUntil = Time.time + 6f;
    }

    void Update()
    {
        if (!playerNearby || unlocked || Time.timeScale == 0f) return;

        GameManager gm = GameManager.Instance;
        if (gm != null && (gm.IsWorking || gm.IsGameOver)) return;

        if (!enteringCode)
        {
            if (hasCode && Input.GetKeyDown(KeyCode.E)) enteringCode = true;
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseKeypad();
            return;
        }

        foreach (char key in Input.inputString)
        {
            if (key >= '0' && key <= '9' && enteredCode.Length < code.Length)
            {
                enteredCode += key;
                wrongCode = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Backspace) && enteredCode.Length > 0)
            enteredCode = enteredCode.Substring(0, enteredCode.Length - 1);

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            TryUnlock();
    }

    void TryUnlock()
    {
        if (code.Length == 0 || enteredCode != code)
        {
            enteredCode = "";
            wrongCode = true;
            return;
        }

        if (doorBlocker == null || doorBlocker.isTrigger)
        {
            Debug.LogWarning("Assign a solid Collider2D as the door blocker.", this);
            return;
        }

        unlocked = true;
        doorBlocker.enabled = false;
        if (doorSprite != null) doorSprite.enabled = false;
        CloseKeypad();
    }

    void CloseKeypad()
    {
        enteringCode = false;
        enteredCode = "";
        wrongCode = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) playerNearby = true;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        playerNearby = false;
        CloseKeypad();
    }

    void OnGUI()
    {
        if (unlocked || Time.timeScale == 0f) return;

        float left = (Screen.width - 300) / 2f;
        if (Time.time < showNoteUntil)
            GUI.Box(new Rect(left, 20, 300, 28), "Code note collected. Door code: " + code);

        if (!playerNearby) return;

        if (!enteringCode)
        {
            string prompt = hasCode ? "Press E to enter the door code" : "Locked. Find the code note.";
            GUI.Box(new Rect(left, Screen.height - 55, 300, 28), prompt);
            return;
        }

        float top = (Screen.height - 120) / 2f;
        GUI.Box(new Rect(left, top, 300, 120), "Door keypad");
        GUI.Label(new Rect(left + 15, top + 26, 270, 22), "Code: " + enteredCode);
        GUI.Label(new Rect(left + 15, top + 48, 270, 22), "Enter to submit   |   Esc to close");
        string note = Input.GetKey(KeyCode.Tab) ? "Code note: " + code : "Hold Tab to read the code note";
        GUI.Label(new Rect(left + 15, top + 70, 270, 22), note);
        if (wrongCode)
            GUI.Label(new Rect(left + 15, top + 92, 270, 22), "Incorrect code. Try again.");
    }
}
