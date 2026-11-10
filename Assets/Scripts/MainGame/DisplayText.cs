using TMPro;
using UnityEngine;

public class DisplayText : MonoBehaviour
{
    public TMP_Text documentCount;   // normal HUD: documents kept so far
    public TMP_Text reviewCount;     // inside the desk: documents waiting for review

    void Update()
    {
        GameManager gm = GameManager.Instance;
        if (gm == null) return;

        if (documentCount) documentCount.text = "Documents: " + gm.docsKept + " / " + gm.docsNeeded;
        if (reviewCount) reviewCount.text = "To review: " + gm.docsToReview;
    }
}
