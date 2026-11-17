using UnityEngine;


// A trigger area. Subclasses add own behaviour by overriding OnPlayerEnter / OnPlayerExit.
//Rooms require a collider to help detect when the player enters the room and leave the room for the specific effects to take place
[RequireComponent(typeof(Collider2D))]


public class Room : MonoBehaviour
{
    public string roomName = "Room";
    public roomRestrictions restrictions = roomRestrictions.None;   // what the player can't do while inside
    public Color gizmoColour = Color.cyan;
    protected bool playerInside;// flag to know and check if player is inside a room



    protected virtual void Reset()
    {
        GetComponent<Collider2D>().isTrigger = true;   // runs when the component is first added
    }

    //function to trigger if the player is inside the room. Checks the tag or the bool flag
    void OnTriggerEnter2D(Collider2D c)
    {
        if (!c.CompareTag("Player") || playerInside) return;
        playerInside = true;
        OnPlayerEnter();
    }

    //function to turn off restrictions when player outside the room
    void OnTriggerExit2D(Collider2D c)
    {
        if (!c.CompareTag("Player") || !playerInside) return;
        playerInside = false;
        OnPlayerExit();
    }


    // if the room is switched off or destroyed while the player is inside, still lift its restrictions
    void OnDisable()
    {
        if (playerInside)
        {
            playerInside = false;
            OnPlayerExit();
        }
    }

    //add restriction to the room when the player enters
    protected virtual void OnPlayerEnter()
    {
        Debug.Log("Entered " + roomName);
        GameManager.Instance.AddRestrictions(restrictions);
    }

    //removes restriction to the room when the player enters
    protected virtual void OnPlayerExit()
    {
        Debug.Log("Left " + roomName);
        if (GameManager.Instance != null)
            GameManager.Instance.RemoveRestrictions(restrictions);
    }

    void OnDrawGizmos()
    {
        Collider2D c = GetComponent<Collider2D>();
        if (!c) return;
        Gizmos.color = gizmoColour;
        Gizmos.DrawWireCube(c.bounds.center, c.bounds.size);
    }


}
