using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinUI : MonoBehaviour {

    private Button continueBtn;

    private void Awake() {
        continueBtn = transform.Find("continueBtn").GetComponent<Button>();
        continueBtn.onClick.AddListener(() => {
            SceneManager.Load(SceneManager.Scene.WinScene);
        });
    }

    private void Start() {
        GameHandler.Instance.OnWin += Instance_OnWin;
        Hide();
    }

    private void Instance_OnWin(object sender, System.EventArgs e) {
        Time.timeScale = 0f;
        Show();
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

}
