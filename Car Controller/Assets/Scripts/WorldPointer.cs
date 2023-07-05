using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WorldPointer : MonoBehaviour {

    public static WorldPointer Instance { get; private set; }

    private TextMeshPro textMeshPro;
    private Func<Vector3> getPointerPosition;

    private void Awake() {
        Instance = this;

        textMeshPro = transform.Find("text").GetComponent<TextMeshPro>();
        Hide();
    }

    private void Update() {
        Vector3 dirToPointer = (getPointerPosition() - Player.Instance.GetPosition()).normalized;
        transform.eulerAngles = new Vector3(0, -Utils.GetAngleFromVector(dirToPointer), 0);
        transform.position = Player.Instance.GetPosition() + dirToPointer * 10f;

        textMeshPro.transform.eulerAngles = new Vector3(90, 0, 0);
    }

    public void Show(string textString, Func<Vector3> getPointerPosition) {
        this.getPointerPosition = getPointerPosition;
        textMeshPro.text = textString;
        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

}
