using UnityEngine;
using UnityEngine.EventSystems;


//code for document comparison taken from https://github.com/hacktic-dev/ReturnToSender

public class DraggableDocument : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public float finalScale = 1f;

    RectTransform rt;
    Canvas canvas;
    bool appearing = true;

    void Awake()
    {
        rt = (RectTransform)transform;
        canvas = GetComponentInParent<Canvas>();
    }

    void OnEnable()
    {
        // pop-in animation each time the paper appears
        transform.localScale = new Vector3(0.001f, 0.001f, 1f);
        appearing = true;
    }

    void Update()
    {
        if (!appearing) return;

        //scaling documents to fit the size of the screen and UI canvas
        if (transform.localScale.x < finalScale)
        {
            float s = Time.unscaledDeltaTime * 3.5f;   // unscaled, so it still animates if the game is paused
            transform.localScale += new Vector3(s, s, 0);
        }
        else
        {
            transform.localScale = new Vector3(finalScale, finalScale, 1f);
            appearing = false;
        }
    }

    bool CanDrag()
    {
        return DocumentManager.instance != null && DocumentManager.instance.state == EState.Working;
    }

    public void OnBeginDrag(PointerEventData e)
    {
        if (!CanDrag()) return;
        transform.SetAsLastSibling();   // bring to last item dragged to the front
    }

    public void OnDrag(PointerEventData e)
    {
        if (!CanDrag()) return;
        rt.anchoredPosition += e.delta / canvas.scaleFactor;   // scaleFactor keeps it under the cursor at any resolution
    }

    public void OnEndDrag(PointerEventData e) { }
}