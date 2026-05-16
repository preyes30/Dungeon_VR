using UnityEngine;
using UnityEngine.AI;

public class Door : MonoBehaviour
{
    public string requiredKey;
    public bool isOpen = false;

    public DoorMessage doorMessage; // UI reference
    private bool messageShown = false;

    private NavMeshObstacle obstacle;

    void Start()
    {
        obstacle = GetComponent<NavMeshObstacle>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (isOpen) return;

        // Check for key (XR setups need GetComponentInParent)
        KeyA key = other.GetComponentInParent<KeyA>();

        if (key != null)
        {
            Debug.Log("KEY DETECTED: " + key.keyID);

            if (key.keyID == requiredKey)
            {
                OpenDoor();
                Destroy(other.gameObject);
                return;
            }
        }

        // Player hits door without key
        if (other.CompareTag("Player") && !messageShown)
        {
            doorMessage.ShowMessage();
            messageShown = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            messageShown = false;
        }
    }

    void OpenDoor()
    {
        isOpen = true;

        // Rotate door open
        transform.Rotate(0, 90, 0);

        Debug.Log("OPENING DOOR");

        // Disable NavMesh obstacle so enemies can pass
        if (obstacle != null)
        {
            obstacle.enabled = false;
        }
    }
}