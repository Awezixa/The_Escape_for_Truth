using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

// The keypad popup for CodeLockedDoor. There is one keypad in the scene and every locked door uses it.
// The world keeps running while it is open (guards, cameras, suspicion), only the player stands still.
public class DoorKeypadUI : MonoBehaviour
{
    [SerializeField] GameObject keypadPanel;   // keypad image, display and the ten buttons
    [SerializeField] TMP_Text displayText;     // the screen at the top of the keypad
    [SerializeField] GameObject notePanel;     // the collected note, shown while Tab is held
    [SerializeField] TMP_Text noteText;        // the code written on the note

    [SerializeField] float wrongCodePenalty = 0.15f;   // suspicion added for each wrong code (0.15 = 15%)
    [SerializeField] float errorTime = 0.8f;           // seconds "ERROR" shows before the entry clears
    [SerializeField] int codeLength = 4;               // the code is checked as soon as this many digits are in

    [SerializeField] Color normalTextColour = new Color(0.55f, 1f, 0.6f);
    [SerializeField] Color errorTextColour = new Color(1f, 0.3f, 0.3f);

    CodeLockedDoor door;   // the door the keypad is open for (null when closed)
    string enteredCode = "";
    bool showingError;
    float errorUntil;

    // the cursor state before the keypad opened, so it can be put back
    bool oldCursorVisible;
    CursorLockMode oldCursorLock;

    public bool IsOpen => door != null;
    public bool IsOpenFor(CodeLockedDoor lockedDoor) => door != null && door == lockedDoor;

    void Start()
    {
        if (keypadPanel) keypadPanel.SetActive(false);
        if (notePanel) notePanel.SetActive(false);
    }

    public void Open(CodeLockedDoor lockedDoor)
    {
        if (IsOpen || lockedDoor == null) return;

        door = lockedDoor;
        enteredCode = "";
        showingError = false;
        RefreshDisplay();

        if (noteText) noteText.text = "DOOR CODE\n<size=170%>" + door.Code + "</size>";   // the code from the door itself
        if (keypadPanel) keypadPanel.SetActive(true);
        if (notePanel) notePanel.SetActive(false);

        // make sure the mouse can be used, and remember how it was
        oldCursorVisible = Cursor.visible;
        oldCursorLock = Cursor.lockState;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (GameManager.Instance != null) GameManager.Instance.SetUsingKeypad(true);   // player stands still
    }

    public void Close()
    {
        if (!IsOpen) return;

        door = null;
        enteredCode = "";
        showingError = false;

        if (keypadPanel) keypadPanel.SetActive(false);
        if (notePanel) notePanel.SetActive(false);

        Cursor.visible = oldCursorVisible;
        Cursor.lockState = oldCursorLock;

        if (GameManager.Instance != null) GameManager.Instance.SetUsingKeypad(false);
    }

    void Update()
    {
        if (!IsOpen) return;

        // caught while typing: close the keypad and let the normal game over take over
        GameManager gm = GameManager.Instance;
        if (gm != null && gm.IsGameOver)
        {
            Close();
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Close();
            return;
        }

        // after a wrong code, wait a moment, then clear the screen for another try
        if (showingError && Time.time >= errorUntil)
        {
            showingError = false;
            enteredCode = "";
            RefreshDisplay();
        }

        if (notePanel) notePanel.SetActive(Input.GetKey(KeyCode.Tab));
    }

    // called by each keypad button's OnClick with its digit (0-9)
    public void PressDigit(int digit)
    {
        // stop Space/Enter from "clicking" the last button again through UI navigation
        if (EventSystem.current != null) EventSystem.current.SetSelectedGameObject(null);

        if (!IsOpen || showingError || Time.timeScale == 0f) return;
        if (enteredCode.Length >= codeLength) return;

        enteredCode += digit.ToString();   // a string, so codes like 0427 keep their leading zero
        RefreshDisplay();

        if (enteredCode.Length >= codeLength) CheckCode();
    }

    void CheckCode()
    {
        if (door.TryCode(enteredCode))
        {
            Close();
            return;
        }

        // wrong code: show an error, block clicks for a moment and add suspicion once
        showingError = true;
        errorUntil = Time.time + errorTime;

        if (displayText)
        {
            displayText.text = "ERROR";
            displayText.color = errorTextColour;
        }

        SuspicionMeter meter = FindFirstObjectByType<SuspicionMeter>();
        if (meter != null) meter.AddSuspicion(wrongCodePenalty);
    }

    // shows the digits typed so far, with _ for the ones still missing, e.g. "0 4 _ _"
    void RefreshDisplay()
    {
        if (!displayText) return;

        string text = "";
        for (int i = 0; i < codeLength; i++)
        {
            if (i > 0) text += " ";
            text += i < enteredCode.Length ? enteredCode[i].ToString() : "_";
        }

        displayText.text = text;
        displayText.color = normalTextColour;
    }

    // if this object is switched off or the scene unloads, never leave the player stuck
    void OnDisable()
    {
        Close();
    }
}
