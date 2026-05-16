using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic;

public class GunInput : MonoBehaviour
{
    public Gun gun;

    void Update()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Right, devices);

        foreach (var device in devices)
        {
            if (device.TryGetFeatureValue(CommonUsages.triggerButton, out bool pressed) && pressed)
            {
                gun.Shoot();
            }
        }
    }
}