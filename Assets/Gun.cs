using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Gun : MonoBehaviour
{
    public Transform muzzle;
    public float range = 50f;
    public int damage = 25;

    public AudioClip gunShotSound;
    private AudioSource audioSource;

    public bool isHeld = false;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        grab = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
    }

    void OnEnable()
    {
        if (grab != null)
            grab.selectEntered.AddListener(OnGrab);
    }

    void OnDisable()
    {
        if (grab != null)
            grab.selectEntered.RemoveListener(OnGrab);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        isHeld = true;
        Debug.Log("Gun picked up");
    }

    public void Shoot()
    {

        if (gunShotSound != null && audioSource != null)
        {
            audioSource.pitch = Random.Range(0.95f, 1.05f);
            audioSource.PlayOneShot(gunShotSound);
        }

        // VISUAL DEBUG
        //Debug.DrawRay(muzzle.position, muzzle.forward * range, Color.red, 2f);

        RaycastHit hit;

        if (Physics.Raycast(muzzle.position, muzzle.forward, out hit, range))
        {
            Debug.Log("Hit: " + hit.collider.name);

            Monster monster = hit.collider.GetComponentInParent<Monster>();

            if (monster != null)
            {
                Debug.Log("Monster found!");
                monster.TakeDamage(damage);
            }
            else
            {
                Debug.Log("No Monster script found on hit object");
            }
        }
    }

    public void TriggerShoot()
    {
        if (!isHeld) return;

        Shoot();
    }
}