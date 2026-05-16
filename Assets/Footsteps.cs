using UnityEngine;

public class Footsteps : MonoBehaviour
{
    public CharacterController controller;
    public AudioClip footstepSound;

    private AudioSource audioSource;

    public float stepInterval = 0.5f;
    private float stepTimer;

    public float movementThreshold = 0.5f; 

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
{
    if (controller == null) return;

    Vector3 velocity = controller.velocity;
    velocity.y = 0f;

    float speed = velocity.magnitude;

    
    if (speed > movementThreshold)
    {
        stepTimer -= Time.deltaTime;

        if (stepTimer <= 0f)
        {
            PlayFootstep();

            
            stepTimer = Mathf.Max(stepTimer + stepInterval, stepInterval);
        }
    }
    else
    {
      
        stepTimer = Mathf.Min(stepTimer, stepInterval);
    }
}

    void PlayFootstep()
    {
        if (footstepSound != null && audioSource != null)
        {
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(footstepSound);
        }
    }
}