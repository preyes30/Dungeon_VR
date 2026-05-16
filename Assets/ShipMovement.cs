using UnityEngine;
using UnityEngine.XR;

public class ShipMovement : MonoBehaviour
{
    [Header("Speed")]
    public float maxSpeed = 8f;
    public float acceleration = 3f;
    public float deceleration = 2f;

    [Header("Turning")]
    public float turnSpeed = 60f;
    public float pitchSpeed = 35f;
    public float bankAmount = 25f;

    [Header("Vertical")]
    public float verticalSpeed = 3f;

    [Header("Head (Camera)")]
    public Transform head;

    [Header("Audio")]
    public AudioSource engineAudio;
    public float minPitch = 0.8f;
    public float maxPitch = 1.5f;

    public bool canFly = false;

    private float currentSpeed = 0f;

    void Update()
    {
        if (!canFly) return;

        InputDevice rightHand = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        Vector2 input;
        rightHand.TryGetFeatureValue(CommonUsages.primary2DAxis, out input);

        float forwardInput = input.y;
        float turnInput = input.x;

        // 🚀 Smooth acceleration
        if (Mathf.Abs(forwardInput) > 0.1f)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, forwardInput * maxSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, deceleration * Time.deltaTime);
        }

        // 🚀 Move forward
        transform.position += transform.forward * currentSpeed * Time.deltaTime;

        // 🌀 Yaw (turn)
        transform.Rotate(Vector3.up, turnInput * turnSpeed * Time.deltaTime);

        // ✈️ Pitch from head
        if (head != null)
        {
            float look = head.forward.y;

            if (Mathf.Abs(look) > 0.2f)
            {
                transform.Rotate(Vector3.right, -look * pitchSpeed * Time.deltaTime);
            }
        }

        // 🧠 Clamp pitch
        float pitch = transform.eulerAngles.x;
        if (pitch > 180) pitch -= 360;

        pitch = Mathf.Clamp(pitch, -45f, 45f);

        Vector3 angles = transform.eulerAngles;
        angles.x = pitch;

        // ✈️ Banking (roll)
        float roll = -turnInput * bankAmount;
        angles.z = roll;

        transform.eulerAngles = angles;

        // 🚀 Vertical movement (head assist)
        float vertical = 0f;

        if (head != null)
        {
            float look = head.forward.y;
            if (Mathf.Abs(look) > 0.2f)
            {
                vertical += look;
            }
        }

        transform.position += Vector3.up * vertical * verticalSpeed * Time.deltaTime;

        /// 🔊 ENGINE SOUND (SMOOTH)
        if (engineAudio != null)
        {
        float speedPercent = Mathf.Abs(currentSpeed) / maxSpeed;

        // Always keep playing once started
         if (!engineAudio.isPlaying)
         {
        engineAudio.Play();
         }

        // Smooth pitch change
         engineAudio.pitch = Mathf.Lerp(minPitch, maxPitch, speedPercent);

         // Smooth volume (idle hum → loud engine)
         engineAudio.volume = Mathf.Lerp(0.2f, 1f, speedPercent);
}

        // 📳 HAPTICS (controller vibration)
        float vibration = Mathf.Abs(currentSpeed) / maxSpeed;
        SendHaptics(vibration * 0.5f);
    }

    void SendHaptics(float intensity)
    {
        InputDevice rightHand = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        if (rightHand.isValid)
        {
            rightHand.SendHapticImpulse(0, intensity, 0.1f);
        }
    }
}