using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuUI : MonoBehaviour {

    private void Awake() {
        transform.Find("playBtn").GetComponent<Button>().onClick.AddListener(() => {
            SceneManager.Load(SceneManager.Scene.GameScene);
        });

        transform.Find("quitBtn").GetComponent<Button>().onClick.AddListener(() => {
            Application.Quit();
        });

        transform.Find("codeMonkeyBtn").GetComponent<Button>().onClick.AddListener(() => {
            Application.OpenURL("https://www.youtube.com/c/CodeMonkeyUnity?sub_confirmation=1");
        });

        Time.timeScale = 1f;
    }

}
