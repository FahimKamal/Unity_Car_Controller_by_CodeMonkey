﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour {

    private Transform mainCameraTransform;

    private void Awake() {
        mainCameraTransform = Camera.main.transform;
    }

    private void Update() {
        transform.LookAt(mainCameraTransform.position);
    }

    private void OnEnable() {
        transform.LookAt(mainCameraTransform.position);
    }

}
