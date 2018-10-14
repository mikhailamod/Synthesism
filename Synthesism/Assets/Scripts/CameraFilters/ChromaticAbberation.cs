using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class ChromaticAbberation : RetroShader
{

    public float minROffset;
    public float maxROffset;
    public float minGOffset;
    public float maxGOffset;
    public float minBOffset;
    public float maxBOffset;


    public float factor = 1;

    public PlayerCarController controller;

    protected override void ApplyMaterial()
    {
        if(controller != null)
        {
            material.SetFloat("_rOffset", Mathf.Lerp(minROffset, maxROffset, controller.carMovementProperties.GetSpeed() / controller.carMovementProperties.maxSpeed));
            material.SetFloat("_gOffset", Mathf.Lerp(minGOffset, maxGOffset, controller.carMovementProperties.GetSpeed() / controller.carMovementProperties.maxSpeed));
            material.SetFloat("_bOffset", Mathf.Lerp(minBOffset, maxBOffset, controller.carMovementProperties.GetSpeed() / controller.carMovementProperties.maxSpeed));
            
        }
        else
        {
            material.SetFloat("_rOffset", minROffset);
            material.SetFloat("_gOffset", minGOffset);
            material.SetFloat("_bOffset", minBOffset);
        }
        material.SetFloat("_Factor", factor);
    }

    protected override Shader GetShader()
    {
        return AssetDatabase.LoadAssetAtPath<Shader>("Assets/Shaders/ChromaticAbberation.shader");
    }
}
