using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class ChromaticAbberation : RetroShader
{

    public float rOffset;
    public float gOffset;
    public float bOffset;


    public float factor = 1;

    protected override void ApplyMaterial()
    {
        material.SetFloat("_rOffset", rOffset);
        material.SetFloat("_gOffset", gOffset);
        material.SetFloat("_bOffset", bOffset);
            
        material.SetFloat("_Factor", factor);
    }

    protected override Shader GetShader()
    {
        return Resources.Load<Shader>("Assets/Shaders/ChromaticAbberation.shader");
    }
}
