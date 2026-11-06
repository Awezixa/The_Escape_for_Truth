using UnityEngine;

public class WorkStation : MonoBehaviour
{
    public bool isWorking;
    private bool playerNearby;

    void Update()
    {
        isWorking = false;

        if (Time.timeScale == 0f) return;

        // Work only when nearby, holding F and not pressing movement keys.
        if (playerNearby && Input.GetKey(KeyCode.F))
        {
            if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
            {
                isWorking = true;
            }
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
            isWorking = false;
        }
    }

    void OnDisable()
    {
        playerNearby = false;
        isWorking = false;
    }
}