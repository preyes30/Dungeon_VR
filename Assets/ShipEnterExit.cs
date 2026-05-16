using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class ShipEnterExit : MonoBehaviour
{
    public Transform seatPosition;
    public GameObject xrOrigin;

    private bool isInside = false;
    private bool isNearShip = false;
    private bool wasPressedLastFrame = false;
    private bool lockToSeat = false;

    private CharacterController cc;
    private ContinuousMoveProviderBase moveProvider;
    private SnapTurnProviderBase turnProvider;

    void Start()
    {
        Debug.Log("SCRIPT IS RUNNING");

        cc = xrOrigin.GetComponent<CharacterController>();
        moveProvider = xrOrigin.GetComponent<ContinuousMoveProviderBase>();
        turnProvider = xrOrigin.GetComponent<SnapTurnProviderBase>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.CompareTag("Player"))
        {
            Debug.Log("Player is near ship");
            isNearShip = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform.root.CompareTag("Player"))
        {
            Debug.Log("Player left ship");
            isNearShip = false;
        }
    }

    void Update()
    {
        // A button (right controller)
        InputDevice rightHand = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        bool aPressed = false;
        rightHand.TryGetFeatureValue(CommonUsages.primaryButton, out aPressed);

        bool justPressed = aPressed && !wasPressedLastFrame;

        if (!isInside && isNearShip && justPressed)
        {
            Debug.Log("ENTERING SHIP");
            EnterShip();
        }
        else if (isInside && justPressed)
        {
            Debug.Log("EXITING SHIP");
            ExitShip();
        }

        wasPressedLastFrame = aPressed;
    }

    void LateUpdate()
    {
        if (!lockToSeat) return;

        Transform camera = xrOrigin.GetComponentInChildren<Camera>().transform;

        // Keep head aligned to seat
        Vector3 offset = xrOrigin.transform.position - camera.position;
        xrOrigin.transform.position = seatPosition.position + offset;
    }

    void EnterShip()
    {
        cc.enabled = false;

        // Align head (camera) to seat
        Transform camera = xrOrigin.GetComponentInChildren<Camera>().transform;
        Vector3 offset = xrOrigin.transform.position - camera.position;

        xrOrigin.transform.position = seatPosition.position + offset + Vector3.up * 0.1f;
        xrOrigin.transform.rotation = seatPosition.rotation;

        cc.enabled = true;

        // Anchor player to ship
        xrOrigin.transform.SetParent(transform);

        // Lock player in seat
        lockToSeat = true;

        // Disable player movement
        if (moveProvider) moveProvider.enabled = false;
        if (turnProvider) turnProvider.enabled = false;

        // Enable ship movement
        ShipMovement ship = GetComponent<ShipMovement>();
        if (ship) ship.canFly = true;

        isInside = true;
    }

    void ExitShip()
    {
        cc.enabled = false;

        // Un-parent player
        xrOrigin.transform.SetParent(null);

        // Move player slightly outside ship
        xrOrigin.transform.position += xrOrigin.transform.forward * 2f;

        cc.enabled = true;

        // Unlock seat
        lockToSeat = false;

        // Re-enable player movement
        if (moveProvider) moveProvider.enabled = true;
        if (turnProvider) turnProvider.enabled = true;

        // Disable ship movement
        ShipMovement ship = GetComponent<ShipMovement>();
        if (ship) ship.canFly = false;

        isInside = false;
    }
}