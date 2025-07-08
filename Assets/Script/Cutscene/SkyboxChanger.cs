using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxChanger : MonoBehaviour
{
    public Material skyboxMaterial;
    private Material previousSkybox;

    void OnEnable()
    {
        previousSkybox = RenderSettings.skybox;
        RenderSettings.skybox = skyboxMaterial;
        DynamicGI.UpdateEnvironment();
    }

    void OnDisable()
    {
        RenderSettings.skybox = previousSkybox;
        DynamicGI.UpdateEnvironment();
    }
}
