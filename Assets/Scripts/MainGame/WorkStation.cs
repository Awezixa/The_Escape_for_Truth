using Unity.VisualScripting;
using UnityEngine;

public class WorkStation : MonoBehaviour
{
    public bool isWorking;
    private bool playerNearby;
    private bool wasWorking;

    private float workingCD = 10.0f;
    private float _nextWorkTime;

    public bool IsCoolingDown => Time.time < _nextWorkTime;


    void Update()
    {
        //kind of have working cooldown for working
        if (IsCoolingDown) return;


        // you can only be working if the player is standing still, holding f (going to change to toggling f) and the game isnt paused or over
        bool working = Time.timeScale != 0f && playerNearby && Input.GetKey(KeyCode.F) && Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0;

        isWorking = working;

        if (Time.timeScale == 0f) return;

        if (working != wasWorking)
        {
            wasWorking = working;
            GameManager.Instance.SetWorking(working);
            StartCooldown();
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

        // Update no longer runs once disabled, so close the UI here
        if (wasWorking && GameManager.Instance != null)
            GameManager.Instance.SetWorking(false);
        wasWorking = false;
    }

    //need fiure out cooldown implementation
    private void StartCooldown()
    {
        _nextWorkTime = Time.time + workingCD;
    }


}