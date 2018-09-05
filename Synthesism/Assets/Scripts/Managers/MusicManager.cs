using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoSingleton<MusicManager> {

    public AudioSource backgroundSong;

    public void PlaySong()
    {
        backgroundSong.Play();
    }
}
