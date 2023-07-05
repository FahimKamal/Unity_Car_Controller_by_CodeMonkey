using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneManager {

    public enum Scene {
        GameScene,
        WinScene,
        MainMenuScene,
    }

    public static void Load(Scene scene) {
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene.ToString());
    }

}
