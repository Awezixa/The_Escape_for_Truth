using UnityEngine;
using UnityEngine.InputSystem;

public class WaypointFollow : MonoBehaviour
{
    [SerializeField] private WaypointPath path;
    [SerializeField] private float speed = 2f;
    [SerializeField] private bool loop = true;
    [SerializeField] private float clickRadius = 0.5f;

    private int index;
    private bool done;

    private void Update()
    {
        if (path == null || path.Count == 0) return;

        if (Mouse.current.leftButton.wasPressedThisFrame) PickNode();
        if (done) return;

        transform.position = Vector3.MoveTowards(transform.position, path.Get(index), speed * Time.deltaTime);


        //loop logic for ai to follow nodes in set order till reach end, can make infite if change order
        if (transform.position != path.Get(index)) return;

        if (index + 1 < path.Count) index++;
        else if (loop) index = 0;
        else done = true;

    }


    private void PickNode()
    {
        Vector3 click = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        for (int i = 0; i < path.Count; i++)
        {
            if (Vector2.Distance(click, path.Get(i)) > clickRadius) continue;
            index = i;
            done = false;
            break;
        }


    }
}
