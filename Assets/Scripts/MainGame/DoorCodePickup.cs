using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class DoorCodePickup : MonoBehaviour
{
    [SerializeField] CodeLockedDoor door;
    bool playerNearby;

    void Reset()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    void Update()
    {
        if (!playerNearby || !Input.GetKeyDown(KeyCode.E) || Time.timeScale == 0f) return;

        GameManager gm = GameManager.Instance;
        if (gm != null && (!gm.CanCollect || gm.IsWorking || gm.IsGameOver)) return;

        if (door == null)
        {
            Debug.LogWarning("Assign a Code Locked Door to this pickup.", this);
            return;
        }

        door.CollectCode();
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) playerNearby = true;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) playerNearby = false;
    }

    void OnGUI()
    {
        if (playerNearby && Time.timeScale != 0f)
            GUI.Box(new Rect((Screen.width - 300) / 2f, Screen.height - 90, 300, 28), "Press E to pick up the code note");
    }
}
