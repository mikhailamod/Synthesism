using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoSingleton<MusicManager> {
    [System.Serializable]
    public struct AudioClipName
    {
        public string name;
        public AudioClip audio;
    }

    private int currentSong;
    public AudioClip[] songs;
    public AudioSource backgroundSong;
    public AudioSource currentSoundEffect;

    public AudioClipName[] soundEffectClips;
    Dictionary<string, AudioClip> soundEffects;

    private void Start()
    {
        currentSong = 0;
        backgroundSong.clip = songs[currentSong];

        soundEffects = new Dictionary<string, AudioClip>();
        //work around to view dictionary in inspector
        foreach(AudioClipName a in soundEffectClips)
        {
            soundEffects.Add(a.name, a.audio);
        }
    }

    private void Update()
    {
        if(!backgroundSong.isPlaying)
        {
            nextSongIndex();
            backgroundSong.clip = songs[currentSong];
            StartMusic();
        }
    }

    public void StartMusic()
    {
        backgroundSong.Play();
    }

    public void PlaySoundEffect(string key)
    {
        resetEffectPitch();//force pitch of AudioSource to be 1f
        currentSoundEffect.clip = soundEffects[key];
        currentSoundEffect.Play();
    }
    public void PlaySoundEffectOnce(string key)
    {
        resetEffectPitch();//force pitch of AudioSource to be 1f
        currentSoundEffect.clip = soundEffects[key];
        if(!currentSoundEffect.isPlaying)
        {
            currentSoundEffect.Play();
        }
    }

    public void PlaySoundEffect(string key, float pitch)
    {
        currentSoundEffect.clip = soundEffects[key];
        currentSoundEffect.pitch = pitch;
        currentSoundEffect.Play();
    }

    public void resetEffectPitch()
    {
        currentSoundEffect.pitch = 1f;
    }

    private void nextSongIndex()
    {
        currentSong++;
        if(currentSong >= songs.Length)
        {
            currentSong = 0;
        }
    }
}
