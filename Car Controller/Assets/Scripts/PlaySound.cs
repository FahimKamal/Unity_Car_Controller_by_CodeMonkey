using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour {

    private static Dictionary<string, AudioSource> soundNameAudioSourceDic = new Dictionary<string, AudioSource>();

    public static void Play(string soundName) {
        soundNameAudioSourceDic[soundName].Play();
    }


    private AudioSource audioSource;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
        soundNameAudioSourceDic[transform.name] = audioSource;
    }

    private void Start() {
        Options.OnVolumeChanged += Options_OnVolumeChanged;
        UpdateVolume();
    }

    private void Options_OnVolumeChanged(object sender, System.EventArgs e) {
        UpdateVolume();
    }

    private void UpdateVolume() {
        float soundVolume = -30 + 50 * Options.soundVolume;
        audioSource.outputAudioMixerGroup.audioMixer.SetFloat("soundVolume", soundVolume);
    }

    public void Play() {
        audioSource.Play();
    }

}
