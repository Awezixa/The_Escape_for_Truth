using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayText : MonoBehaviour
{
    public TMP_Text documentCount;
    public TMP_Text keptCount;

    // Update is called once per frame
    void Update()
    {
        docsCollected();
        docsKept();
        
    }


    void docsCollected()
    {
        documentCount.text = "Documents: " + GameManager.Instance.docsCollected;
    }

    void docsKept()
    {
        keptCount.text = "Kept: " + GameManager.Instance.docsKept;
        //GameManager.Instance.docsCollected = GameManager.Instance.docsKept;
    }



}
