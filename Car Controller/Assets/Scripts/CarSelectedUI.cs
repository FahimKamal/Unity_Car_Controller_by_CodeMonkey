using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CarSelectedUI : MonoBehaviour {

    private Transform visualTransform;
    private TextMeshProUGUI amountText;

    private void Awake() {
        visualTransform = transform.Find("visual");
        amountText = visualTransform.Find("amountText").GetComponent<TextMeshProUGUI>();
    }

    private void Start() {
        PlayerSelection.Instance.OnSelectedCarListChanged += Instance_OnSelectedCarListChanged;
        UpdateVisual();
    }

    private void Instance_OnSelectedCarListChanged(object sender, System.EventArgs e) {
        UpdateVisual();
    }

    private void UpdateVisual() {
        visualTransform.gameObject.SetActive(PlayerSelection.Instance.GetSelectedCarAIListCount() > 0);
        amountText.text = "x" + PlayerSelection.Instance.GetSelectedCarAIListCount().ToString();
    }

}
