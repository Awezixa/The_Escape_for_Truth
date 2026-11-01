//using UnityEngine;

//public class WorkStation : MonoBehaviour
//{
//    [SerializeField] ComparisonDesk desk;
//    bool playerNearby;

//    void Update()
//    {
//        if (playerNearby && Input.GetKeyDown(KeyCode.E) && GameManager.Instance.State == GameState.Playing)
//            desk.Open();
//    }

//    void OnTriggerEnter2D(Collider2D c) { if (c.CompareTag("Player")) playerNearby = true; }
//    void OnTriggerExit2D(Collider2D c) { if (c.CompareTag("Player")) playerNearby = false; }
//}