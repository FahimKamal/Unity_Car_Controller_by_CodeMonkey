using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WinSceneUI : MonoBehaviour {

    private void Awake() {
        transform.Find("mainMenuBtn").GetComponent<Button>().onClick.AddListener(() => {
            SceneManager.Load(SceneManager.Scene.MainMenuScene);
        });

        Time.timeScale = 1f;
    }

    private void Start() {
        TimeSpan gameTimeSpan = TimeSpan.FromSeconds(GameWinStats.gameTime);
        transform.Find("gameTimeText").GetComponent<TextMeshProUGUI>().text = gameTimeSpan.ToString(@"mm\:ss\.fff");

        transform.Find("statsText").GetComponent<TextMeshProUGUI>().text =
            GameWinStats.resourcesGathered + "\n" +
            GameWinStats.zombiesKilled + "\n" +
            GameWinStats.trackCheckpoints + "\n" +
            GameWinStats.carsBuilt
        ;
    }

}
