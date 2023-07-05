using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour {

    private void Awake() {
        transform.Find("musicBtn").GetComponent<Button>().onClick.AddListener(() => {
            Options.ChangeMusicVolume();
        });

        transform.Find("soundBtn").GetComponent<Button>().onClick.AddListener(() => {
            Options.ChangeSoundVolume();
        });
    }
}
