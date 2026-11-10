using UnityEngine;

public enum EState { Start, Working, End }

public class DocumentManager : MonoBehaviour
{
    [SerializeField] DocumentView[] documents;   // the papers that can be on the desk at once
    [SerializeField] DocumentGenerator generator;
    public static DocumentManager instance = null;

    public EState state { get; private set; } = EState.Start;
    public DocumentView current { get; private set; }

    bool[] dealt;   // dealt[i] = paper i currently holds a document. Array for the amount of the documents to be delt

    void Awake()
    {
        if (instance == null) instance = this;
        else { Destroy(this); return; }

        dealt = new bool[documents.Length];//instanciates the amount of documents in the array to see how many need to be reviewed
        foreach (DocumentView d in documents) d.gameObject.SetActive(false);   // desk starts empty
    }

    public void SetState(EState s) => state = s;
    public void Select(DocumentView doc) => current = doc;

    //function to open when player goes to work
    public void OpenDesk()
    {
        int needed = Mathf.Min(documents.Length, GameManager.Instance.docsToReview);// checks the minimum amount of documents needed to be instanciated

        // papers already on the desk keep their documents
        for (int i = 0; i < documents.Length && CountOnDesk() < needed; i++)
        {
            if (!dealt[i]) Deal(i);// instanciates the need amount of docs based on the number of docs collected
        }

        if (current == null) SelectDefault();
    }

    public void KeepCurrent()
    {
        if (!CanDecide()) return;
        if (!GameManager.Instance.KeepDocument()) return;
        Resolve();
    }

    public void ForgetCurrent()
    {
        if (!CanDecide()) return;
        if (!GameManager.Instance.DiscardDocument()) return;
        Resolve();
    }

    bool CanDecide()
    {
        return state == EState.Working && current != null
               && System.Array.IndexOf(documents, current) >= 0;
    }

    // removes the decided paper, and puts a new document on it only if more are still waiting
    void Resolve()
    {
        int idx = System.Array.IndexOf(documents, current);
        current = null;

        dealt[idx] = false;
        documents[idx].gameObject.SetActive(false);

        // documents already on the desk (the decided paper no longer counts)
        //loop to continuiosly add docs based on if there are more documents for the player to review
        if (CountOnDesk() < GameManager.Instance.docsToReview)
            Deal(idx);

        SelectDefault();
    }

    void Deal(int i)
    {
        //generation of documents
        generator.Generate(documents[i]);
        dealt[i] = true;
        documents[i].gameObject.SetActive(true);   // pop-in animation plays
    }

    int CountOnDesk()
    {
        int n = 0;
        for (int i = 0; i < dealt.Length; i++) if (dealt[i]) n++;
        return n;
    }

    // so Keep and Discard work without clicking when only one paper is showing
    void SelectDefault()
    {
        for (int i = 0; i < dealt.Length; i++)
        {
            if (dealt[i]) { current = documents[i]; return; }
        }
        current = null;
    }
}