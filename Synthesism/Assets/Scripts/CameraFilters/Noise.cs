using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class Noise : RetroShader
{
    public float intensity;
    public Texture noise;

    protected override void ApplyMaterial()
    {
        material.SetFloat("_Intensity", intensity);
        material.SetTexture("_NoiseTex", noise);
    }

    protected override Shader GetShader()
    {
        return AssetDatabase.LoadAssetAtPath<Shader>("Assets/Shaders/Noise.shader");
    }
}
