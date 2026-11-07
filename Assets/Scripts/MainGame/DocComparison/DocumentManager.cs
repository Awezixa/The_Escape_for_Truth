
using UnityEngine;

public enum EState { Start, Working, End }

public class DocumentManager : MonoBehaviour
{
    [SerializeField] DocumentView[] documents;
    [SerializeField] DocumentGenerator generator;
    public static DocumentManager instance = null;

    public EState state { get; private set; } = EState.Start;
    public DocumentView current { get; private set; }



    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }
    void Start()
    {
        NewShift();
    }

    public void NewShift()
    {
        foreach (DocumentView d in documents) generator.Generate(d);
    }
    public void SetState(EState s) => state = s;

    // called by a document when the player clicks or drags it
    public void Select(DocumentView doc) => current = doc;

    // hooked up to the Forget button
    public void ForgetCurrent()
    {
        if (state != EState.Working || current == null) return;

        // placeholder rule: forgetting a valid document is a mistake, forgetting a bad one is correct
        //bool correct = !current.isValid;
        //Debug.Log("Forgot a document. Valid = " + current.isValid + ", correct choice = " + correct);

        //if (!correct && meter != null) meter.AddSuspicion(wrongChoicePenalty);

        // reuse the paper: hide it, give it new text, show it again
        DocumentView doc = current;
        current = null;
        doc.gameObject.SetActive(false);
        generator.Generate(doc);
        doc.gameObject.SetActive(true);   // the pop-in animation plays again
    }


}