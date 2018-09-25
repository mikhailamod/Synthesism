using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class Distortion : RetroShader
{
    public float scale = 1;
    public float offset = 0;
    [Range(-10, 10)]
    public float animationSpeed;

    protected override void ApplyMaterial()
    {
        material.SetFloat("_scale", scale);
        material.SetFloat("_offset", offset);
        material.SetFloat("_AnimationSpeed", animationSpeed);
    }

    protected override Shader GetShader()
    {
        return AssetDatabase.LoadAssetAtPath<Shader>("Assets/Shaders/Distortion.shader");
    }
}
