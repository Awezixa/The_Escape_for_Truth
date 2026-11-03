using UnityEngine;

public class WaypointPath : MonoBehaviour
{
    public int Count => transform.childCount;
    public Vector3 Get(int index) => transform.GetChild(index % Count).position;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        for (int i = 0; i < Count; i++)
        {
            Gizmos.DrawSphere(Get(i), 0.1f);
            Gizmos.DrawLine(Get(i), Get(i + 1));
        }
    }


}

