//using TMPro;
//using UnityEngine;

//public class DocumentView : MonoBehaviour
//{
//    [SerializeField] TMP_Text titleText;
//    [SerializeField] Transform fieldContainer;
//    [SerializeField] FieldSlot fieldPrefab;

//    public void Show(DocumentRecord record, ComparisonDesk desk)
//    {
//        titleText.text = record.title;
//        foreach (var f in record.fields)
//            Instantiate(fieldPrefab, fieldContainer).Setup(f, desk);
//    }
//}
