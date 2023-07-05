using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IdleTrucksUI : MonoBehaviour {

    private Transform container;
    private TextMeshProUGUI text;

    private void Awake() {
        container = transform.Find("container");
        container.gameObject.SetActive(false);

        text = container.Find("text").GetComponent<TextMeshProUGUI>();
    }

    private void Update() {
        int truckIdleCount = CarResourceGathererAI.GetIdleCount();
        if (truckIdleCount > 0) {
            container.gameObject.SetActive(true);
            text.text = "x" + truckIdleCount;
        } else {
            container.gameObject.SetActive(false);
        }
    }
}
