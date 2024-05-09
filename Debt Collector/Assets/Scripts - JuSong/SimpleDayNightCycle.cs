using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleDayNightCycle : MonoBehaviour
{
    public float speed = 4.0f;
    public Material dayMaterial;
    public Material nightMaterial;
    private Light directionalLight;

    void Start()
    {
        directionalLight = GetComponent<Light>();
    }

    void Update()
    {
        transform.Rotate(Time.deltaTime * speed, 0, 0);
        UpdateEnvironmentSettings();
    }

    void UpdateEnvironmentSettings()
    {
        float xRotation = transform.localEulerAngles.x;
        
        if (xRotation > 180) xRotation -= 360;

        // Determine if it's day or night based on the xRotation
        if (xRotation > -90 && xRotation < 90)
        {
            // Daytime: between sunrise and sunset
            RenderSettings.skybox = dayMaterial;
            // Adjust the light intensity to smoothly transition between day and night
            directionalLight.intensity = Mathf.Lerp(0.3f, 1.0f, (xRotation + 90) / 180);
            // Optionally change light color
            directionalLight.color = Color.Lerp(new Color(0.05f, 0.05f, 0.2f), Color.white, (xRotation + 90) / 180);
        }
        else
        {
            // Night time: sunset to sunrise
            RenderSettings.skybox = nightMaterial;
            directionalLight.intensity = Mathf.Lerp(1.0f, 0.3f, (xRotation + 90) / -180);
            directionalLight.color = Color.Lerp(Color.white, new Color(0.05f, 0.05f, 0.2f), (xRotation + 90) / -180);
        }

        // Update the skybox material changes in the scene
        DynamicGI.UpdateEnvironment();
    }
}

