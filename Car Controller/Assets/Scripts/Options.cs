using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Options {

    public static event EventHandler OnVolumeChanged;

    public static void CleanUp() {
        OnVolumeChanged = null;
    }



    public static float musicVolume = .25f;
    public static float soundVolume = .25f;


    public static void ChangeMusicVolume() {
        musicVolume = (musicVolume + .25f) % 1f;
        OnVolumeChanged?.Invoke(null, EventArgs.Empty);
    }

    public static void ChangeSoundVolume() {
        soundVolume = (soundVolume + .25f) % 1f;
        OnVolumeChanged?.Invoke(null, EventArgs.Empty);
    }

}
