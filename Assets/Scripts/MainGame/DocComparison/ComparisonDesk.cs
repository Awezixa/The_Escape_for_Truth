//using System.Collections.Generic;
//using UnityEngine;

//public class ComparisonDesk : MonoBehaviour
//{
//    [SerializeField] GameObject deskPanel;
//    [SerializeField] RectTransform documentArea;
//    [SerializeField] DocumentView documentPrefab;

//    Case current;
//    FieldSlot firstPick;
//    readonly HashSet<string> flaggedKeys = new HashSet<string>();

//    public void Open()
//    {
//        deskPanel.SetActive(true);
//        GameManager.Instance.SetState(GameState.Working);
//        NewCase();
//    }

//    public void Close()
//    {
//        deskPanel.SetActive(false);
//        GameManager.Instance.SetState(GameState.Playing);
//    }

//    void NewCase()
//    {
//        foreach (Transform child in documentArea) Destroy(child.gameObject);
//        flaggedKeys.Clear();
//        firstPick = null;

//        current = CaseGenerator.Create();
//        float x = -200f;
//        foreach (var doc in current.documents)
//        {
//            var view = Instantiate(documentPrefab, documentArea);
//            ((RectTransform)view.transform).anchoredPosition = new Vector2(x, 0);
//            view.Show(doc, this);
//            x += 400f;
//        }
//    }

//    // click one field, then a second one: the pair is flagged as a suspected mismatch
//    public void FieldClicked(FieldSlot slot)
//    {
//        if (firstPick == null)
//        {
//            firstPick = slot;
//            slot.SetColour(Color.yellow);
//            return;
//        }

//        if (slot != firstPick && slot.Field.key == firstPick.Field.key)
//        {
//            flaggedKeys.Add(slot.Field.key);
//            slot.SetColour(new Color(1f, 0.6f, 0.2f));
//            firstPick.SetColour(new Color(1f, 0.6f, 0.2f));
//        }
//        else
//        {
//            firstPick.SetColour(Color.white);
//        }
//        firstPick = null;
//    }

//    // hooked up to the Approve and Deny buttons
//    public void Verdict(bool approve)
//    {
//        bool correct = approve == current.shouldApprove;
//        // optional: if denying, also check flaggedKeys matches the real mismatches
//        GameManager.Instance.CaseResolved(correct);
//        NewCase();
//    }
//}