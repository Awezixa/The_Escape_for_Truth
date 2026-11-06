using UnityEngine;
using UnityEngine.UI;

public class SuspicionMeter : MonoBehaviour
{
    public Image[] segments;

    public float riseSpeed = 0.4f;
    public float workSpeed = 0.15f;

    [Range(0f, 1f)] public float suspicion;
    public bool caught;

    private CameraBehaviour[] cameras;
    private WorkStation[] desks;

    void Start()
    {
        cameras = FindObjectsByType<CameraBehaviour>(FindObjectsSortMode.None);
        desks = FindObjectsByType<WorkStation>(FindObjectsSortMode.None);
        UpdateSegments();
    }

    // Runs after the desk and camera Update functions.
    void LateUpdate()
    {
        if (caught) return;

        if (IsPlayerWorking())
        {
            suspicion -= workSpeed * Time.deltaTime;
        }
        else if (IsPlayerSeen())
        {
            suspicion += riseSpeed * Time.deltaTime;
        }

        // There is no automatic decrease when the player is hiding.
        suspicion = Mathf.Clamp01(suspicion);
        UpdateSegments();
        CheckIfCaught();
    }

    bool IsPlayerWorking()
    {
        foreach (WorkStation desk in desks)
        {
            if (desk != null && desk.isWorking) return true;
        }
        return false;
    }

    public bool IsPlayerSeen()
    {
        foreach (CameraBehaviour cam in cameras)
        {
            if (cam != null && cam.isActiveAndEnabled && cam.playerDetected) return true;
        }
        return false;
    }

    public void AddSuspicion(float amount)
    {
        if (caught) return;

        suspicion = Mathf.Clamp01(suspicion + amount);
        UpdateSegments();
        CheckIfCaught();
    }

    void CheckIfCaught()
    {
        if (suspicion >= 1f && !caught)
        {
            caught = true;
            Debug.Log("CAUGHT!");

            if (GameManager.Instance != null)
            {
                GameManager.Instance.PlayerCaught();
            }
        }
    }

    void UpdateSegments()
    {
        float filled = suspicion * segments.Length;

        for (int i = 0; i < segments.Length; i++)
        {
            if (segments[i] == null) continue;

            Color c = segments[i].color;
            c.a = Mathf.Clamp01(filled - i);
            segments[i].color = c;
        }
    }
}