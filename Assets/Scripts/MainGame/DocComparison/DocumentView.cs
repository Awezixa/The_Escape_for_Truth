using System.Net;
using UnityEngine;


public enum EDocumentType
{
    //types of docs we going to have player investigate
    Real,
    Fake,
    Printed,
}





public class DocumentView : MonoBehaviour
{
    string information;
    public bool isValid { get; private set; }
    public EDocumentType documentType { get; private set; }
    public SpriteRenderer documentSprite;
    public TextMesh text;

    public void Initialise(string _information, bool _isInfoValid, EDocumentType _type, float z)
    {
        float rotation = (Random.Range(0f, 1f) > .5f ? 1 : -1) * Random.Range(0f, 20f);
        float stampRotation = (Random.Range(0f, 1f) > .5f ? 1 : -1) * Random.Range(0f, 8f) + rotation;

        transform.rotation = Quaternion.Euler(0, 0, rotation);
        transform.position = new Vector3(Random.Range(-4.5f, 4.5f), Random.Range(-2.5f, 2.5f), -z - .5f);
        text.transform.position = new Vector3(text.transform.position.x, text.transform.position.y, -z - .5f);

        information = _information;
        isValid = _isInfoValid;
        documentType = _type;


        text.text = information;

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
