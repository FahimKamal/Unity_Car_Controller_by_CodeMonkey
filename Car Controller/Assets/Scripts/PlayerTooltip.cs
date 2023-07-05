using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerTooltip : MonoBehaviour {

    public static PlayerTooltip Instance { get; private set; }

    [SerializeField] private Transform tooltip;
    private TextMeshProUGUI text;
    private float timer;

    private void Awake() {
        Instance = this;

        text = tooltip.Find("text").GetComponent<TextMeshProUGUI>();
        Hide();
    }

    private void Update() {
        timer -= Time.deltaTime;
        if (timer <= 0f) {
            Hide();
        }
    }

    public void Show(string textString, float timer = 1.5f) {
        this.timer = timer;
        text.text = textString;
        Show();
    }

    private void Show() {
        tooltip.gameObject.SetActive(true);
    }

    private void Hide() {
        tooltip.gameObject.SetActive(false);
    }


}
