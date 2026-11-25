using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PressurePlate : MonoBehaviour
{
    [Header("What can press it")]
    [SerializeField] string[] acceptedTags = { "Movable", "Throwable" };   // must match tags that exist in your project
    [SerializeField] LayerMask detectMask = ~0;                             // optional: restrict to your object layers
    [SerializeField] float requiredMass = 0f;                               // total mass needed, 0 = anything accepted works
    [SerializeField] bool needsCentreOnPlate = true;                        // object's centre must be over the plate

    [Header("Look")]
    [SerializeField] SpriteRenderer plateSprite;
    [SerializeField] Color idleColour = Color.white;
    [SerializeField] Color pressedColour = Color.green;

    public bool IsPressed { get; private set; }

    BoxCollider2D box;
    ContactFilter2D filter;
    readonly List<Collider2D> hits = new List<Collider2D>();
    readonly HashSet<Rigidbody2D> pressers = new HashSet<Rigidbody2D>();

    void Reset()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    void Awake()
    {
        box = GetComponent<BoxCollider2D>();
        filter = new ContactFilter2D();
        filter.useLayerMask = true;
        filter.layerMask = detectMask;
        filter.useTriggers = false;   // ignore other triggers (document pickups, rooms, this plate)
    }

    void FixedUpdate()
    {
        pressers.Clear();
        float totalMass = 0f;

        int count = Physics2D.OverlapBox(box.bounds.center, box.bounds.size, transform.eulerAngles.z, filter, hits);
        for (int i = 0; i < count; i++)
        {
            Rigidbody2D rb = hits[i].attachedRigidbody;
            if (rb == null) continue;
            if (rb.bodyType != RigidbodyType2D.Dynamic) continue;     // a carried object is kinematic, so it must be put down
            if (!HasAcceptedTag(rb)) continue;
            if (needsCentreOnPlate && !box.OverlapPoint(rb.worldCenterOfMass)) continue;

            if (pressers.Add(rb)) totalMass += rb.mass;               // a set, so an object with 2 colliders counts once
        }

        bool pressed = pressers.Count > 0 && totalMass >= requiredMass;
        if (pressed != IsPressed)
        {
            IsPressed = pressed;
            if (plateSprite) plateSprite.color = pressed ? pressedColour : idleColour;
            Debug.Log(name + (pressed ? " pressed" : " released") + " (mass " + totalMass + ")");
        }
    }

    bool HasAcceptedTag(Rigidbody2D rb)
    {
        foreach (string t in acceptedTags)
            if (rb.tag == t) return true;
        return false;
    }
}