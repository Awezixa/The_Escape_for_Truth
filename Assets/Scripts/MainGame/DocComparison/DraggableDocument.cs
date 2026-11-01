//using UnityEngine;
//using UnityEngine.EventSystems;

//[RequireComponent(typeof(CanvasGroup))]
//public class DraggableDocument : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
//{
//    RectTransform rt;
//    Canvas canvas;
//    CanvasGroup group;

//    void Awake()
//    {
//        rt = (RectTransform)transform;
//        canvas = GetComponentInParent<Canvas>();
//        group = GetComponent<CanvasGroup>();
//    }

//    public void OnBeginDrag(PointerEventData e)
//    {
//        transform.SetAsLastSibling();   // bring to front
//        group.alpha = 0.85f;
//    }

//    // dividing by scaleFactor keeps the paper under the cursor at any resolution
//    public void OnDrag(PointerEventData e) => rt.anchoredPosition += e.delta / canvas.scaleFactor;

//    public void OnEndDrag(PointerEventData e) => group.alpha = 1f;
//}
