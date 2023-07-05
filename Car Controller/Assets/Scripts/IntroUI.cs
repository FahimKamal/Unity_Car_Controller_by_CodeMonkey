using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroUI : MonoBehaviour {

    private void Awake() {
        transform.Find("continueBtn").GetComponent<Button>().onClick.AddListener(() => {
            Time.timeScale = 1f;
            Hide();
        });
    }

    private void Start() {
        Time.timeScale = 0f;
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}
