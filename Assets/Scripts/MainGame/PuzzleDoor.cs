using UnityEngine;

public class PuzzleDoor : MonoBehaviour
{
    [SerializeField] PressurePlate[] plates;     // every plate listed must be pressed
    [SerializeField] Collider2D doorBlocker;     // a solid collider, not a trigger
    [SerializeField] SpriteRenderer doorSprite;
    [SerializeField] bool stayOpen = true;       // once opened, it stays open even if the plate is released

    bool isOpen;

    void Start()
    {
        if (plates == null || plates.Length == 0)
            Debug.LogWarning(name + ": no plates assigned, so this door will never open.", this);
        if (doorBlocker == null || doorBlocker.isTrigger)
            Debug.LogWarning(name + ": assign a solid (non-trigger) Collider2D as the door blocker.", this);
    }

    void Update()
    {
        if (plates == null || plates.Length == 0) return;
        if (isOpen && stayOpen) return;

        bool allPressed = true;
        foreach (PressurePlate p in plates)
        {
            if (p == null || !p.IsPressed) { allPressed = false; break; }
        }

        if (allPressed != isOpen) SetOpen(allPressed);
    }

    void SetOpen(bool open)
    {
        isOpen = open;
        if (doorBlocker) doorBlocker.enabled = !open;
        if (doorSprite) doorSprite.enabled = !open;
        Debug.Log(name + (open ? " opened" : " closed"));
    }
}
