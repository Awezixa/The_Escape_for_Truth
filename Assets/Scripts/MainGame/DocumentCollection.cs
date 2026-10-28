using UnityEngine;

public class DocumentCollection : MonoBehaviour
{
    private bool playerNearby = false;

    void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            collectDocument();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNearby = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNearby = false;
        }
    }

    void collectDocument()
    {
        OnCollected(); // function that does differnt things based on type of document collected
        Debug.Log("Document Collected!");
        Destroy(gameObject);
    }

    protected virtual void OnCollected()
    {

    }

}
