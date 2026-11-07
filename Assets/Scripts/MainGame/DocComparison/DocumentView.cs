using TMPro;
using UnityEngine;
using UnityEngine.UI;


public enum EDocumentType
{
    //types of docs we going to have player investigate
    Real,
    Fake,
    Printed,
}



public class DocumentView : MonoBehaviour
{
    public TMP_Text text;  // main text on documents
    public string information { get; private set; }
    public bool isValid { get; private set; }
    public EDocumentType documentType { get; private set; }
    public SpriteRenderer documentSprite;


    Vector2 startPosition;
    bool startSaved;


    public void Initialise(string _information, bool _isInfoValid, EDocumentType _type)
    {
        RectTransform rt = (RectTransform)transform;

        // remember where it was laid out in the editor, so repeated calls don't drift away
        if (!startSaved)
        {
            startPosition = rt.anchoredPosition;
            startSaved = true;
        }

        information = _information;
        isValid = _isInfoValid;
        documentType = _type;


        if (text) text.text = information;

        //choose doc texture
        //need review when have more sprites and see if we want do this

        //int index = Mathf.RoundToInt(Random.Range(0.5f, 4.49f));

        //int specialLetter = Mathf.RoundToInt(Random.Range(0.5f, 9.49f));

        //Texture2D tex;

        //if (specialLetter == 1)
        //{
        //    index = Mathf.RoundToInt(Random.Range(4.5f, 6.49f));
        //    tex = Resources.Load("letter" + index.ToString(), typeof(Texture2D)) as Texture2D;
        //}
        //else
        //{
        //    tex = Resources.Load("letter" + index.ToString(), typeof(Texture2D)) as Texture2D;
        //}

        ////adjust collision for thinner letters
        //if (index == 1 || index == 3 || index == 4)
        //{
        //    GetComponent<BoxCollider2D>().size = new Vector2(GetComponent<BoxCollider2D>().size.x, 3.5f);
        //}

        //letterSprite.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f));

    }




}
