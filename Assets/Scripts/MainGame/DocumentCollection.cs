using UnityEngine;

public class DocumentCollection : MonoBehaviour
{
    private bool playerNearby = false;
    [Range(0f, 1f)] public float suspicionWhenSeen = 0.15f;   // fraction of the bar, 0.15 = 15%


    // subclasses can override this: should collecting it count towards the level goal?
    protected virtual bool CountsTowardsGoal => true;

    void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            collectDocument();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNearby = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNearby = false;
        }
    }

    void collectDocument()
    {
        if (CountsTowardsGoal) GameManager.Instance.DocumentPickUp();

        ApplySeenPenalty();   // shared rule, using this document's own number
        OnCollected(); // function that does differnt things based on type of document collected
        Debug.Log("Document Collected!");
        Destroy(gameObject);
    }

    void ApplySeenPenalty()
    {
        SuspicionMeter meter = FindFirstObjectByType<SuspicionMeter>();
        if (meter != null && meter.IsPlayerSeen())
        {
            meter.AddSuspicion(suspicionWhenSeen);
            Debug.Log("Seen collecting! Suspicion +" + (suspicionWhenSeen * 100f) + "%");
        }
    }


    protected virtual void OnCollected()
    {

    }

}
