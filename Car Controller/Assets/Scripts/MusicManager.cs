using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

    private AudioSource audioSource;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start() {
        Options.OnVolumeChanged += Options_OnVolumeChanged;
        UpdateVolume();
    }

    private void Options_OnVolumeChanged(object sender, System.EventArgs e) {
        UpdateVolume();
    }

    private void UpdateVolume() {
        float musicVolume = -30 + 50 * Options.musicVolume;
        audioSource.outputAudioMixerGroup.audioMixer.SetFloat("musicVolume", musicVolume);
    }

}
