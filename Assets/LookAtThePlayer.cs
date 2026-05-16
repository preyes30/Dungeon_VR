using UnityEngine;

public class FollowPlayerUI : MonoBehaviour
{
    public Transform playerCamera;
    public float distance = 2f;
    public float heightOffset = 0f;

    void LateUpdate()
    {
        Vector3 targetPosition = playerCamera.position + playerCamera.forward * distance;
        targetPosition.y += heightOffset;

        transform.position = targetPosition;

        // Make it face the player
        transform.LookAt(playerCamera);
        transform.Rotate(0, 180, 0);
    }
}