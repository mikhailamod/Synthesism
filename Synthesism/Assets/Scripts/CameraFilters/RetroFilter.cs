using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class RetroFilter : MonoBehaviour
{
    public RetroProfile profile;

    void OnDisable()
    {
        /*
        Destroy(profile.noise.Material);
        Destroy(profile.scanLines.Material);
        Destroy(profile.chromo.Material);*/
    }

    public void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        Graphics.Blit(src, src, profile.noise.Material);
        Graphics.Blit(src, src, profile.scanLines.Material);
        Graphics.Blit(src, dst, profile.chromo.Material);
    }
}
