using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GunLock : MonoBehaviour
{
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab;
    private UnityEngine.XR.Interaction.Toolkit.Interactors.IXRSelectInteractor lockedHand = null;

    void Awake()
    {
        grab = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
    }

    void OnEnable()
    {
        grab.selectEntered.AddListener(OnGrab);
        grab.selectExited.AddListener(OnRelease);
    }

    void OnDisable()
    {
        grab.selectEntered.RemoveListener(OnGrab);
        grab.selectExited.RemoveListener(OnRelease);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        // First hand that grabs becomes owner
        if (lockedHand == null)
        {
            lockedHand = args.interactorObject;
            Debug.Log("Gun locked to: " + lockedHand.transform.name);
        }

        // If another hand tries to grab → kick it out
        else if (args.interactorObject != lockedHand)
        {
            grab.interactionManager.SelectExit(args.interactorObject, grab);
        }
    }

    void OnRelease(SelectExitEventArgs args)
    {
        // Prevent dropping from the locked hand
        if (args.interactorObject == lockedHand)
        {
            grab.interactionManager.SelectEnter(lockedHand, grab);
        }
    }
}