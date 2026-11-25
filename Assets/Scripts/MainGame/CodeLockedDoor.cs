using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class CodeLockedDoor : MonoBehaviour
{
    [SerializeField] string code = "0427";
    [SerializeField] Collider2D doorBlocker;
    [SerializeField] SpriteRenderer doorSprite;
    [SerializeField] DoorKeypadUI keypad;   // the keypad popup in the scene

    bool playerNearby;
    bool hasCode;
    bool unlocked;

    public string Code => code;

    void Reset()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    void Start()
    {
        if (keypad == null) keypad = FindFirstObjectByType<DoorKeypadUI>();
    }

    public void CollectCode()
    {
        hasCode = true;
    }

    void Update()
    {
        if (!playerNearby || unlocked || Time.timeScale == 0f) return;

        GameManager gm = GameManager.Instance;
        if (gm != null && (gm.IsWorking || gm.IsGameOver || gm.IsUsingKeypad)) return;

        // the note has to be collected before the keypad can be opened
        if (hasCode && Input.GetKeyDown(KeyCode.E) && keypad != null)
            keypad.Open(this);
    }

    // called by the keypad once all the digits are in. Returns true if the door opened.
    public bool TryCode(string enteredCode)
    {
        if (code.Length == 0 || enteredCode != code) return false;

        if (doorBlocker == null || doorBlocker.isTrigger)
        {
            Debug.LogWarning("Assign a solid Collider2D as the door blocker.", this);
            return false;
        }

        unlocked = true;
        doorBlocker.enabled = false;
        if (doorSprite != null) doorSprite.enabled = false;
        return true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) playerNearby = true;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        playerNearby = false;
        if (keypad != null && keypad.IsOpenFor(this)) keypad.Close();
    }

    void OnDisable()
    {
        if (keypad != null && keypad.IsOpenFor(this)) keypad.Close();
    }

    void OnGUI()
    {
        if (unlocked || !playerNearby || Time.timeScale == 0f) return;
        if (keypad != null && keypad.IsOpen) return;

        float left = (Screen.width - 300) / 2f;
        string prompt = hasCode ? "Press E to enter the door code" : "Locked. Find the code note.";
        GUI.Box(new Rect(left, Screen.height - 55, 300, 28), prompt);
    }
}
