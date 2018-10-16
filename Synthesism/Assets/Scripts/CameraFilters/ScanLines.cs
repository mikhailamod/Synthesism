using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class ScanLines : RetroShader
{

    public float tiling = 1.0f;
    public float contrast = 0.5f;

    protected override void ApplyMaterial()
    {
        material.SetFloat("_Tiling", tiling);
        material.SetFloat("_Contrast", contrast);
    }

    protected override Shader GetShader()
    {
        return Resources.Load<Shader>("Assets/Shaders/ScanLines.shader");
    }
}
