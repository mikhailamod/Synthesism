using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class RetroShader
{

    protected Shader shader;

    protected Material material;

    public Material Material
    {
        get
        {
            if(!shader)
            {
                shader = GetShader();
            }

            if (!material)
            {
                material = new Material(shader);              
            }
            ApplyMaterial();
            return material;
        }
    }

    protected abstract Shader GetShader();
    protected abstract void ApplyMaterial();
}
