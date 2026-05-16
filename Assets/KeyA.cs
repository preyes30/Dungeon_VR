using UnityEngine;

public class KeyA : MonoBehaviour
{
    public string keyID;

    void OnCollisionEnter(Collision collision)
{
    Debug.Log("KEY HIT: " + collision.gameObject.name);
}
}

