using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="RetroProfile",menuName ="Retro Shaders/Create Profile")]
public class RetroProfile : ScriptableObject
{
    public Noise noise;
    public ScanLines scanLines;
    public ChromaticAbberation chromo;
}
