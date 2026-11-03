// FakeDocument.cs
using UnityEngine;

public class FakeDocument : DocumentCollection
{
    void Reset() { suspicionWhenSeen = 0.30f; }

    // a fake shouldn't help you pass the level
    protected override bool CountsTowardsGoal => false;

    protected override void OnCollected()
    {
        Debug.Log("That was a fake!");
    }
}
