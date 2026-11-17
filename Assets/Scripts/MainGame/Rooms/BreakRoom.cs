using UnityEngine;

public class BreakRoom : Room
{
    //overriding methods for different rooms. Need to adjust each method for each separate room
    protected override void Reset()
    {
        base.Reset();//clear room first
        roomName = "Break Room";
        restrictions = roomRestrictions.Work | roomRestrictions.Sprint | roomRestrictions.Collect;//add each restriction specific to each room
        gizmoColour = Color.green;//can change colours for each room
    }

    protected override void OnPlayerEnter()
    {
        base.OnPlayerEnter();   // applies the restrictions

        //add code and what it will do when player is on the game
        SuspicionMeter meter = FindFirstObjectByType<SuspicionMeter>();
        if (meter != null) meter.ResetSuspicion();
        Debug.Log("Suspicion reset to 0");
    }
}
