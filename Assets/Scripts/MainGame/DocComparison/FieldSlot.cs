//using TMPro;
//using UnityEngine;
//using UnityEngine.EventSystems;
//using UnityEngine.UI;

//public class FieldSlot : MonoBehaviour, IPointerClickHandler
//{
//    [SerializeField] TMP_Text label;
//    [SerializeField] Image background;

//    public DocField Field { get; private set; }
//    ComparisonDesk desk;

//    public void Setup(DocField field, ComparisonDesk d)
//    {
//        Field = field;
//        desk = d;
//        label.text = field.key + ": " + field.value;
//    }

//    public void OnPointerClick(PointerEventData e) => desk.FieldClicked(this);
//    public void SetColour(Color c) => background.color = c;
//}