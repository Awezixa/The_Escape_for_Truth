using UnityEngine;
using UnityEngine.UI;

public class SuspicionMeter : MonoBehaviour
{
    public Image[] segments;

    public float riseSpeed = 0.4f;
    public float drainSpeed = 0.15f;
    public float drainDelay = 1f;

    [Range(0f, 1f)] public float suspicion;
    public bool caught;

    
    //cam
    private CameraBehaviour[] cameras;
    private float drainTimer;

    void Start()
    {
        cameras = FindObjectsByType<CameraBehaviour>(FindObjectsSortMode.None);
        if (cameras.Length == 0) Debug.LogWarning("SuspicionMeter: no CameraBehaviour found in the scene.");

        UpdateSegments();
    }

    void Update()
    {
        if (caught) return;

        bool seen = false;
        foreach (CameraBehaviour cam in cameras)
        {
            if (cam && cam.playerDetected) { seen = true; break; }
        }

        if (seen)
        {
            suspicion += riseSpeed * Time.deltaTime;
            drainTimer = drainDelay;
        }
        else if (drainTimer > 0f)
        {
            drainTimer -= Time.deltaTime;
        }
        else
        {
            suspicion -= drainSpeed * Time.deltaTime;
        }

        suspicion = Mathf.Clamp01(suspicion);
        UpdateSegments();

        if (suspicion >= 1f)
        {
            caught = true;
            Debug.Log("CAUGHT!");
        }
    }

    void UpdateSegments()
    {
        float filled = suspicion * segments.Length;

        for (int i = 0; i < segments.Length; i++)
        {
            if (!segments[i]) continue;

            Color c = segments[i].color;
            c.a = Mathf.Clamp01(filled - i);
            segments[i].color = c;
        }
    }
}