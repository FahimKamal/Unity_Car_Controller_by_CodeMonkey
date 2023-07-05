using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineSound : MonoBehaviour {


    [SerializeField] private CarDriver carDriver;
    [SerializeField] private AudioSource audioSource;

    private void Start() {
        Options.OnVolumeChanged += Options_OnVolumeChanged;
        UpdateVolume();
    }

    private void Update() {
        float speedNormalized = Mathf.Clamp01(carDriver.GetSpeed() / 80f);
        audioSource.pitch = .6f + 1f * speedNormalized;
    }

    private void Options_OnVolumeChanged(object sender, System.EventArgs e) {
        UpdateVolume();
    }

    private void UpdateVolume() {
        float soundVolume = -30 + 50 * Options.soundVolume;
        audioSource.outputAudioMixerGroup.audioMixer.SetFloat("soundVolume", soundVolume);
    }

}
