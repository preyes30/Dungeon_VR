using UnityEngine;

public class DoorMessage : MonoBehaviour
{
    public GameObject needKeyText;

    public void ShowMessage()
    {
        needKeyText.SetActive(true);
        CancelInvoke();
        Invoke(nameof(HideMessage), 2f);
    }

    void HideMessage()
    {
        needKeyText.SetActive(false);
    }
}