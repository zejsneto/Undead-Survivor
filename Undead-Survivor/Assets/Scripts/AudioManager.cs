using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance; // Singleton instance of the AudioManager

    [Header("# BGM")]
    public AudioClip bgmClip; // Background music AudioClip
    public float bgmVolume; // Volume of the background music
    AudioSource bgmPlayer; // AudioSource for playing background music
    AudioHighPassFilter bgmEffect; // AudioHighPassFilter for background music effect

    [Header("# SFX")]
    public AudioClip[] sfxClips; // Array of sound effect AudioClips
    public float sfxVolume; // Volume of sound effects
    public int channels; // Number of audio channels for playing sound effects
    AudioSource[] sfxPlayers; // Array of AudioSources for playing sound effects
    int channelIndex; // Index of the current audio channel for playing sound effects

    // Enum representing different sound effects
    public enum Sfx { Dead, Hit, LevelUp = 3, Lose, Melee, Range = 7, Select, Win }

    private void Awake()
    {
        instance = this; // Set the singleton instance to this AudioManager
        Init(); // Initialize the AudioManager
    }

    private void Init()
    {
        // Initialize background music player
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip;
        bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>(); // Get the AudioHighPassFilter from the main camera

        // Initialize sound effect players
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[index].playOnAwake = false;
            sfxPlayers[index].bypassListenerEffects = true;
            sfxPlayers[index].volume = sfxVolume;
        }
    }

    // Play or stop background music based on the boolean parameter
    public void PlayBgm(bool isPlay)
    {
        if (isPlay)
        {
            bgmPlayer.Play(); // Play background music
        }
        else
        {
            bgmPlayer.Stop(); // Stop background music
        }
    }

    // Enable or disable the background music effect based on the boolean parameter
    public void EffectBgm(bool isPlay)
    {
        bgmEffect.enabled = isPlay; // Enable or disable the background music effect
    }

    // Play a sound effect based on the specified enum value
    public void PlaySfx(Sfx sfx)
    {
        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            int loopIndex = (index + channelIndex) % sfxPlayers.Length;

            if (sfxPlayers[loopIndex].isPlaying)
                continue;

            int randIndex = 0;
            if (sfx == Sfx.Hit || sfx == Sfx.Melee)
            {
                randIndex = Random.Range(0, 2); // Generate a random index for variations in hit or melee sound effects
            }

            channelIndex = loopIndex; // Update the current channel index
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx + randIndex]; // Assign the appropriate AudioClip to the AudioSource
            sfxPlayers[loopIndex].Play(); // Play the sound effect
            break;
        }
    }
}
