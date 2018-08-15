using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class RetroFilter : MonoBehaviour
{
    public RetroProfile profile;

    void OnDisable()
    {
        Destroy(profile.noise.Material);
        Destroy(profile.lines.Material);
        Destroy(profile.chromo.Material);
    }

    public void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        Graphics.Blit(src, src, profile.noise.Material);
        Graphics.Blit(src, src, profile.lines.Material);
        Graphics.Blit(src, dst, profile.chromo.Material);
    }
}
