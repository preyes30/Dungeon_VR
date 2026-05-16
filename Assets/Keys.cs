using UnityEngine;

public class Keys : MonoBehaviour
{
    public string keyID;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventory inventory = other.GetComponent<PlayerInventory>();
            inventory.AddKey(keyID);

            Destroy(gameObject);
        }
    }
}