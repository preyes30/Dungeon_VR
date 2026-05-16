using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
{
    Debug.Log("Triggered by: " + other.name);

    if (other.CompareTag("Player"))
    {
        Debug.Log("PLAYER WON");
        FindObjectOfType<GameManager>().WinGame();
    }
}
}